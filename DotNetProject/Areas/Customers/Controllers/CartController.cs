using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.DataAccess.Repository;
using Web.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Web.Models.Models;
using Web.Utilities;
using Stripe;
using Stripe.Checkout;


namespace DotNetProject.Areas.Customers.Controllers
{
    [Area("Customers")]
    [Authorize]
    public class CartController : Controller
    {

        private readonly IUnitOfWork _db;
        [BindProperty]
        public ShoppingCartVM ShoppingCartVM { get; set; }
        public CartController(IUnitOfWork db)
        {
            _db = db;
        }
        // GET: CartController
        public ActionResult Index()
        {
            var ClaimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = ClaimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            ShoppingCartVM = new()
            {
                ShoppingCartList = _db.shoppingCart.GetAll(u => u.ApplicationUserId == userId,
                includeProperties: "Product"),
                OrdersHeader = new()

            };
            foreach (var cart in ShoppingCartVM.ShoppingCartList)
            {
                cart.Price = GetPriceBasedOnQuantity(cart);
                ShoppingCartVM.OrderTotal += (cart.Price * cart.Count);
            }
            return View(ShoppingCartVM);
        }

        public double GetPriceBasedOnQuantity(ShoppingCart shoppingCart)
        {
            if (shoppingCart.Count <= 50)
            {
                return shoppingCart.Product.Price;
            }
            else
            {
                if (shoppingCart.Count <= 100)
                {
                    return shoppingCart.Product.Price50;
                }
                else
                {
                    return shoppingCart.Product.Price100;
                }
            }
        }

        public ActionResult Plus(int cartId)
        {
            var cart = _db.shoppingCart.Get(u => u.Id == cartId);
            cart.Count += 1;
            _db.shoppingCart.Update(cart);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public ActionResult Minus(int cartId)
        {
            var cart = _db.shoppingCart.Get(u => u.Id == cartId);
            cart.Count -= 1;
            if (cart.Count < 1)
            {
                _db.shoppingCart.Remove(cart);
            }
            else
            {
                _db.shoppingCart.Update(cart);
            }
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public ActionResult Remove(int cartId)
        {
            var cart = _db.shoppingCart.Get(u => u.Id == cartId);
            _db.shoppingCart.Remove(cart);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Summary()
        {
            var ClaimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = ClaimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            ShoppingCartVM = new()
            {
                ShoppingCartList = _db.shoppingCart.GetAll(u => u.ApplicationUserId == userId,
                includeProperties: "Product"),
                OrdersHeader = new()

            };
            ShoppingCartVM.OrdersHeader.ApplicationUser = _db.ApplicationUser.Get(u => u.Id == userId);
            ShoppingCartVM.OrdersHeader.Name = ShoppingCartVM.OrdersHeader.ApplicationUser.Name;
            ShoppingCartVM.OrdersHeader.PhoneNumber = ShoppingCartVM.OrdersHeader.ApplicationUser.PhoneNumber;
            ShoppingCartVM.OrdersHeader.StreetAddress = ShoppingCartVM.OrdersHeader.ApplicationUser.StreetAddress;
            ShoppingCartVM.OrdersHeader.City = ShoppingCartVM.OrdersHeader.ApplicationUser.City;
            ShoppingCartVM.OrdersHeader.State = ShoppingCartVM.OrdersHeader.ApplicationUser.State;
            ShoppingCartVM.OrdersHeader.PostalCode = ShoppingCartVM.OrdersHeader.ApplicationUser.PostalCode;

            foreach (var cart in ShoppingCartVM.ShoppingCartList)
            {
                cart.Price = GetPriceBasedOnQuantity(cart);
                ShoppingCartVM.OrdersHeader.OrderTotal += cart.Price * cart.Count;
            }
            return View(ShoppingCartVM);
        }
        [HttpPost]
        [ActionName("Summary")]
        public IActionResult SummaryPost()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            // Get shopping cart items
            ShoppingCartVM.ShoppingCartList = _db.shoppingCart.GetAll(
                u => u.ApplicationUserId == userId,
                includeProperties: "Product"
            );

            // Create order header
            ShoppingCartVM.OrdersHeader.OrderDate = DateTime.Now;
            ShoppingCartVM.OrdersHeader.ApplicationUserId = userId;

            // Calculate total
            foreach (var cart in ShoppingCartVM.ShoppingCartList)
            {
                cart.Price = GetPriceBasedOnQuantity(cart);
                ShoppingCartVM.OrdersHeader.OrderTotal +=
                    (cart.Price * cart.Count);
            }

            // Get user
            ShoppingCartVM.OrdersHeader.ApplicationUser =
                _db.ApplicationUser.Get(u => u.Id == userId);

            // Company user or regular user
            if (ShoppingCartVM.OrdersHeader.ApplicationUser
                .CompanyId.GetValueOrDefault() == 0)
            {
                // Regular customer
                ShoppingCartVM.OrdersHeader.OrderStatus =
                    SD.StatusPending;

                ShoppingCartVM.OrdersHeader.PaymentStatus =
                    SD.PaymentStatusPending;
            }
            else
            {
                // Company customer
                ShoppingCartVM.OrdersHeader.OrderStatus =
                    SD.StatusApproved;

                ShoppingCartVM.OrdersHeader.PaymentStatus =
                    SD.PaymentStatusDelayedPayment;
            }

            // Save OrderHeader
            _db.OrderHeader.Add(ShoppingCartVM.OrdersHeader);
            _db.SaveChanges();

            // Save OrderDetails
            foreach (var cart in ShoppingCartVM.ShoppingCartList)
            {
                OrderDetails orderDetails = new()
                {
                    ProductId = cart.ProductId,
                    OrderHeaderId = ShoppingCartVM.OrdersHeader.Id,
                    Price = cart.Price,
                    Count = cart.Count
                };

                _db.OrderDetails.Add(orderDetails);
            }

            _db.SaveChanges();

            // Stripe payment for regular users
            if (ShoppingCartVM.OrdersHeader.ApplicationUser
                .CompanyId.GetValueOrDefault() == 0)
            {
                // IMPORTANT:
                // Replace this with your ACTUAL forwarded GitHub Codespace URL
                var domain = "https://congenial-space-goggles-649p69x4qjph55rw-5023.app.github.dev/";

                var options = new SessionCreateOptions
                {
                    SuccessUrl = domain +
                        $"customers/cart/OrderConfirmation?id={ShoppingCartVM.OrdersHeader.Id}",

                    CancelUrl = domain + "customers/cart/index",

                    Mode = "payment",

                    LineItems = new List<SessionLineItemOptions>()
                };

                // Add cart items to Stripe session
                foreach (var cart in ShoppingCartVM.ShoppingCartList)
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
                        ShoppingCartVM.OrdersHeader.Id,
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

            // Company users skip Stripe
            return RedirectToAction(
                nameof(OrderConfirmation),
                new { id = ShoppingCartVM.OrdersHeader.Id }
            );
        }

        public IActionResult OrderConfirmation(int id)
        {
            return View(id);
        }
    }
}