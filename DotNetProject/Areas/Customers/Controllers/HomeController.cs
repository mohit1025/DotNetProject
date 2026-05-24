using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DotNetProject.Models;
using Web.DataAccess.Repository;
using Web.Models;
using Web.Models.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Web.Utilities;

namespace DotNetProject.Controllers;

[Area("Customers")]
public class HomeController : Controller
{

    private readonly IUnitOfWork _db;
    private readonly ILogger<HomeController> _logger;
    public HomeController(ILogger<HomeController> logger, IUnitOfWork db)
    {
        _db = db;
        _logger = logger;

    }
    public IActionResult Index()
    {

        var ClaimsIdentity = (ClaimsIdentity)User.Identity;
        var Claim = ClaimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
        if (Claim != null)
        {
            HttpContext.Session.SetInt32(SD.SessionCart, _db.shoppingCart.GetAll(u => u.ApplicationUserId == Claim.Value).Count());
        }
        IEnumerable<Products> objProductList = _db.Product.GetAll(includeProperties: "Category");
        return View(objProductList);
    }

    public IActionResult Details(int Id)
    {
        ShoppingCart cart = new ShoppingCart
        {
            Product = _db.Product.Get(u => u.Id == Id, includeProperties: "Category"),
            Count = 1,
            ProductId = Id
        };
        return View(cart);
    }
    [HttpPost]
    [Authorize]
    public IActionResult Details(ShoppingCart shoppingCart)
    {
        var ClaimsIdentity = (ClaimsIdentity)User.Identity;
        var userId = ClaimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
        shoppingCart.ApplicationUserId = userId;
        shoppingCart.Id = 0;

        ShoppingCart cartFromDb = _db.shoppingCart.Get(u => u.ApplicationUserId == userId && u.ProductId == shoppingCart.ProductId);
        
        if (cartFromDb != null)
        {
            cartFromDb.Count += shoppingCart.Count;
            _db.shoppingCart.Update(cartFromDb);
            _db.SaveChanges();
        }
        else
        {
            _db.shoppingCart.Add(shoppingCart);
            _db.SaveChanges();
            HttpContext.Session.SetInt32(SD.SessionCart, _db.shoppingCart.GetAll(u => u.ApplicationUserId == userId).Count());
        }

       
        return RedirectToAction(nameof(Index));
    }


    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
