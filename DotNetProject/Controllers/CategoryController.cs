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
                _db.Categories.Add(categoryObj);
                _db.SaveChanges();
                TempData["success"]="Category Created Succesfully";
                return RedirectToAction("Index");
            }
            return View();

        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Category category = await _db.Categories.FindAsync(id);
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
                _db.Categories.Update(categoryObj);
                _db.SaveChanges();
                 TempData["success"]="Category Updated Succesfully";
                return RedirectToAction("Index");
            }
            return View();

        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Category category = await _db.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeletePost(int? id)
        {
            Category categoryObj = await _db.Categories.FindAsync(id);
            if (id == null)
            {
                return NotFound();
            }

            _db.Categories.Remove(categoryObj);
            _db.SaveChanges();
             TempData["success"]="Category Deleted Succesfully";
            return RedirectToAction("Index");


        }
    }
}
