using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.DataAccess.Repository;
using Web.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Web.Models.Models;


namespace DotNetProject.Areas.Customers.Controllers
{
    [Area("Customers")]
    [Authorize]
    public class CartController : Controller
    {

         private readonly IUnitOfWork _db;
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
            ShoppingCartVM = new ()
            {
                ShoppingCartList = _db.shoppingCart.GetAll(u => u.ApplicationUserId == userId,
                includeProperties :"Product")
                
            };
            foreach(var cart in ShoppingCartVM.ShoppingCartList)
            {
                cart.Price = GetPriceBasedOnQuantity(cart);
                ShoppingCartVM.OrderTotal += (cart.Price  * cart.Count);
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
           
            return View(Summary);
        }       

    }
}
