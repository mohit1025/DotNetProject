using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DotNetProject.Models;
using Web.DataAccess.Repository;
using Web.Models;

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
        IEnumerable<Products> objProductList = _db.Product.GetAll(includeProperties: "Category");
        return View(objProductList);
    }

    public IActionResult Details(int id)
    {
        Products objProduct = _db.Product.Get(u => u.Id == id, includeProperties: "Category");
        return View(objProduct);
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
