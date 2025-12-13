using System.Data.Common;
using DotNetProject.Data;
using DotNetProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotNetProject.Controllers
{
    public class CategoryController : Controller
    {
        
        private readonly ApplicationDbContext _db;
        public CategoryController(ApplicationDbContext db)
        {
            this._db = db;
        }
        // GET: CategoryController
        public ActionResult Index()
        {
           List<Category> objCategoryList = _db.Categories.ToList();
            return View();
        }
    }
}
