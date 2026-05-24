using System.Data.Common;
using Web.Models;
using DotNetProject.Models;
using Microsoft.AspNetCore.Mvc;
using Web.DataAccess.Repository.IRepository;
using Web.DataAccess.Repository;
using Microsoft.AspNetCore.Authorization;
using Web.Utilities;
using Web.Models.Models;
using Web.Models.ViewModels;
using Stripe;
using Stripe.Checkout;

namespace DotNetProject.Controllers
{
    [Area("Admin")]
    // [Authorize(Roles = SD.Role_Admin)]
    [Authorize]
    public class OrderController : Controller
    {

        private readonly IUnitOfWork _db;
        [BindProperty]
        public OrderVM OrderVM { get; set; }
        public OrderController(IUnitOfWork db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }
        // GET: OrderController
        [HttpGet]
        public IActionResult GetAll()
        {
            List<OrderHeader> objOrderHeaders;

            // ADMIN CAN SEE ALL ORDERS
            if (User.IsInRole(SD.Role_Admin))
            {
                objOrderHeaders = _db.OrderHeader
                    .GetAll(includeProperties: "ApplicationUser")
                    .ToList();
            }
            else
            {
                // NORMAL USER CAN SEE ONLY THEIR ORDERS

                var claimsIdentity =
                    (System.Security.Claims.ClaimsIdentity)User.Identity;

                var userId =
                    claimsIdentity.FindFirst(
                        System.Security.Claims.ClaimTypes.NameIdentifier
                    ).Value;

                objOrderHeaders = _db.OrderHeader
                    .GetAll(
                        u => u.ApplicationUserId == userId,
                        includeProperties: "ApplicationUser"
                    )
                    .ToList();
            }

            return Json(new { data = objOrderHeaders });
        }
        public IActionResult Details(int id)
        {
            OrderVM = new()
            {
                OrderHeader = _db.OrderHeader.Get(
                    u => u.Id == id,
                    includeProperties: "ApplicationUser"
                ),

                OrderDetail = _db.OrderDetails.GetAll(
                    u => u.OrderHeaderId == id,
                    includeProperties: "Product"
                ).ToList()
            };

            if (OrderVM.OrderHeader == null)
            {
                return NotFound();
            }

            // Non-admin users can only access their own orders
            if (!User.IsInRole(SD.Role_Admin))
            {
                var claimsIdentity =
                    (System.Security.Claims.ClaimsIdentity)User.Identity;

                var userId =
                    claimsIdentity.FindFirst(
                        System.Security.Claims.ClaimTypes.NameIdentifier
                    ).Value;

                if (OrderVM.OrderHeader.ApplicationUserId != userId)
                {
                    return Forbid();
                }
            }

            return View(OrderVM);
        }

        [HttpPost]
        [Authorize(Roles = SD.Role_Admin)]
        public IActionResult UpdateOrderDetails(int id)
        {

            var orderHeaderFromDb = _db.OrderHeader.Get(u => u.Id == OrderVM.OrderHeader.Id);
            orderHeaderFromDb.Name = OrderVM.OrderHeader.Name;
            orderHeaderFromDb.PhoneNumber = OrderVM.OrderHeader.PhoneNumber;
            orderHeaderFromDb.StreetAddress = OrderVM.OrderHeader.StreetAddress;
            orderHeaderFromDb.City = OrderVM.OrderHeader.City;
            orderHeaderFromDb.State = OrderVM.OrderHeader.State;
            orderHeaderFromDb.PostalCode = OrderVM.OrderHeader.PostalCode;

            if (!String.IsNullOrEmpty(OrderVM.OrderHeader.Carrier))
            {
                orderHeaderFromDb.Carrier = OrderVM.OrderHeader.Carrier;
            }
            if (!String.IsNullOrEmpty(OrderVM.OrderHeader.TrackingNumber))
            {
                orderHeaderFromDb.TrackingNumber = OrderVM.OrderHeader.TrackingNumber;
            }
            _db.OrderHeader.Update(orderHeaderFromDb);
            _db.SaveChanges();
            TempData["success"] = "Order Details Updated Successfully";
            return RedirectToAction("Details", new { id = orderHeaderFromDb.Id });
        }

        [HttpPost]
        [Authorize(Roles = SD.Role_Admin)]
        public IActionResult ShipOrder()
        {
            var orderHeader = _db.OrderHeader.Get(
                u => u.Id == OrderVM.OrderHeader.Id
            );

            orderHeader.TrackingNumber =
                OrderVM.OrderHeader.TrackingNumber;

            orderHeader.Carrier =
                OrderVM.OrderHeader.Carrier;

            orderHeader.OrderStatus =
                SD.StatusShipped;

            orderHeader.ShippingDate =
                DateTime.Now;

            // For delayed payment / COD
            if (orderHeader.PaymentStatus ==
                    SD.PaymentStatusDelayedPayment)
            {
                orderHeader.PaymentDueDate =
                    DateOnly.FromDateTime(DateTime.Now)
                    .AddDays(30);
            }

            _db.OrderHeader.Update(orderHeader);

            _db.SaveChanges();

            TempData["success"] =
                "Order Shipped Successfully.";

            return RedirectToAction(
                nameof(Details),
                new { id = OrderVM.OrderHeader.Id }
            );
        }

        public IActionResult StartProcessing()
        {
            var orderHeader = _db.OrderHeader.Get(
                u => u.Id == OrderVM.OrderHeader.Id
            );

            orderHeader.OrderStatus =
                SD.StatusInProcess;

            _db.OrderHeader.Update(orderHeader);

            _db.SaveChanges();

            TempData["success"] =
                "Order Status Updated Successfully.";

            return RedirectToAction(
                nameof(Details),
                new { id = OrderVM.OrderHeader.Id }
            );
        }

        public IActionResult CancelOrder()
        {
            var orderHeader = _db.OrderHeader.Get(
                u => u.Id == OrderVM.OrderHeader.Id
            );

            if (orderHeader.PaymentStatus == SD.PaymentStatusApproved)
            {
              var options = new Stripe.RefundCreateOptions
              {
                  Reason = Stripe.RefundReasons.RequestedByCustomer,
                  PaymentIntent = orderHeader.paymentIntentId
              };

                var service = new Stripe.RefundService();
                Refund refund = service.Create(options);
                _db.OrderHeader.UpdateStatus(
                    orderHeader.Id,
                    SD.StatusCancelled,
                    SD.PaymentStatusRefunded
                );
                _db.SaveChanges();
            }
            else
            {
                _db.OrderHeader.UpdateStatus(
                    orderHeader.Id,
                    SD.StatusCancelled,
                    SD.PaymentStatusCancelled
                );
                _db.SaveChanges();
            }

            TempData["success"] =
                "Order Cancelled Successfully.";

            return RedirectToAction(
                nameof(Details),
                new { id = OrderVM.OrderHeader.Id }
            );
        }

        [HttpPost]
        [ActionName("Details")]
        public IActionResult Details_PAY_NOW(int id)
        {
             OrderVM = new()
            {
                OrderHeader = _db.OrderHeader.Get(
                    u => u.Id == id,
                    includeProperties: "ApplicationUser"
                ),

                OrderDetail = _db.OrderDetails.GetAll(
                    u => u.OrderHeaderId == id,
                    includeProperties: "Product"
                ).ToList()
            };

             
                // Replace with your actual domain
                var domain =
                    "https://congenial-space-goggles-649p69x4qjph55rw-5023.app.github.dev/";

                var options = new SessionCreateOptions
                {
                    SuccessUrl = domain +
                        $"admin/order/PaymentConfirmation?id={OrderVM.OrderHeader.Id}",

                    CancelUrl = domain + $"admin/order/Details?id={OrderVM.OrderHeader.Id}",

                    Mode = "payment",

                    LineItems = new List<SessionLineItemOptions>()
                };

                // Add cart items to Stripe session
                foreach (var cart in OrderVM.OrderDetail)
                {
                    var sessionLineItem =
                        new SessionLineItemOptions
                        {
                            PriceData =
                                new SessionLineItemPriceDataOptions
                                {
                                    UnitAmount =
                                        (long)(cart.Price * 100),

                                    Currency = "usd",

                                    ProductData =
                                        new SessionLineItemPriceDataProductDataOptions
                                        {
                                            Name = cart.Product.Title
                                        }
                                },

                            Quantity = cart.Count
                        };

                    options.LineItems.Add(sessionLineItem);
                }

                try
                {
                    var service = new SessionService();

                    Session session = service.Create(options);

                    Console.WriteLine("SESSION ID: " + session.Id);
                    Console.WriteLine("SESSION URL: " + session.Url);

                    // Save Stripe session details
                    _db.OrderHeader.UpdateStripePaymentId(
                        OrderVM.OrderHeader.Id,
                        session.Id,
                        session.PaymentIntentId
                    );

                    _db.SaveChanges();

                   
                    // Redirect user to Stripe Checkout
                    return Redirect(session.Url);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());

                    return Content(ex.ToString());
                }
            

          
        }

        public IActionResult PaymentConfirmation(int id)
        {
            OrderHeader orderHeader = _db.OrderHeader.Get(
        u => u.Id == id
    );

            // For Company order
            if (orderHeader.PaymentStatus == SD.PaymentStatusDelayedPayment)
            {
                var service = new SessionService();

                Session session = service.Get(orderHeader.SessionId);

                // Payment successful
                if (session.PaymentStatus.ToLower() == "paid")
                {
                    _db.OrderHeader.UpdateStripePaymentId(
                        id,
                        session.Id,
                        session.PaymentIntentId
                    );

                    _db.OrderHeader.UpdateStatus(
                        id,
                        orderHeader.OrderStatus,
                        SD.PaymentStatusApproved
                    );

                    _db.SaveChanges();
                }
            }
           
            return View(id);
        }
    }
}

