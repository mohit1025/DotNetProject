using System.Data.Common;
using DotNetProject.Data;
using DotNetProject.Models;
using Microsoft.AspNetCore.Mvc;
using Web.DataAccess.Repository.IRepository;
using Web.DataAccess.Repository;

namespace DotNetProject.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {

        private readonly IUnitOfWork _db;
        public CategoryController(IUnitOfWork db)
        {
            _db= db;
        }
        // GET: CategoryController
        public ActionResult Index()
        {
            List<Category> objCategoryList = _db.Category.GetAll().ToList();
            return View(objCategoryList);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category categoryObj)
        {
            if (categoryObj.Name == categoryObj.ToString())
            {
                ModelState.AddModelError("name", "The display order cannot exactly match the name");
            }
            if (ModelState.IsValid)
            {
                _db.Category.Add(categoryObj);
                _db.SaveChanges();
                TempData["success"] = "Category Created Succesfully";
                return RedirectToAction("Index");
            }
            return View();

        }
        public  IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Category? category =  _db.Category.Get(u => u.Id == id);
            if (category == null)
            {
                return NotFound();
            }


            return View(category);
        }

        [HttpPost]
        public IActionResult Edit(Category categoryObj)
        {
            if (ModelState.IsValid)
            {
                _db.Category.Update(categoryObj);
                _db.SaveChanges();
                TempData["success"] = "Category Updated Succesfully";
                return RedirectToAction("Index");
            }
            return View();

        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Category? category =  _db.Category.Get(u => u.Id ==id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }
            Category? categoryObj =  _db.Category.Get(u=> u.Id==id);
            if (categoryObj == null)
            {
                return NotFound();
            }
            _db.Category.Remove(categoryObj);
            _db.SaveChanges();
            TempData["success"] = "Category Deleted Succesfully";
            return RedirectToAction("Index");


        }
    }
}
