using System.Data.Common;
using DotNetProject.Data;
using DotNetProject.Models;
using Microsoft.AspNetCore.Mvc;
using Web.DataAccess.Repository.IRepository;
using Web.DataAccess.Repository;
using Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Web.Models.ViewModels;

namespace DotNetProject.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {

        private readonly IUnitOfWork _db;
        public ProductsController(IUnitOfWork db)
        {
            _db = db;
        }
        // GET: ProductsController
        public ActionResult Index()
        {
            List<Products> objList = _db.Product.GetAll().ToList();

            return View(objList);
        }
        public IActionResult Create()
        {
            IEnumerable<SelectListItem> categoryList = _db.Category.GetAll().Select(i =>
            new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
           
            ProductVM productVM = new()
            {
                Products = new Products(),
                CategoryList = categoryList
            };
            return View(productVM);
        }

        [HttpPost]
        public IActionResult Create(ProductVM productObj)
        {
            if (productObj.Products.Title == productObj.ToString())
            {
                ModelState.AddModelError("title", "The display order cannot exactly match the title");
            }
            if (ModelState.IsValid)
            {
                _db.Product.Add(productObj.Products);
                _db.SaveChanges();
                TempData["success"] = "Product Created Successfully";
                return RedirectToAction("Index");
            }
            // else
            // {
            //     IEnumerable<SelectListItem> categoryList = _db.Category.GetAll().Select(i =>
            //     new SelectListItem
            //     {
            //         Text = i.Name,
            //         Value = i.Id.ToString()
            //     });
              
            // }
            return View(productObj);

        }
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Products? product = _db.Product.Get(u => u.Id == id);
            if (product == null)
            {
                return NotFound();
            }


            return View(product);
        }

        [HttpPost]
        public IActionResult Edit(Products productObj)
        {
            if (ModelState.IsValid)
            {
                _db.Product.Update(productObj);
                _db.SaveChanges();
                TempData["success"] = "Product Updated Successfully";
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

            Products? product = _db.Product.Get(u => u.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }
            Products? productObj = _db.Product.Get(u => u.Id == id);
            if (productObj == null)
            {
                return NotFound();
            }
            _db.Product.Remove(productObj);
            _db.SaveChanges();
            TempData["success"] = "Product Deleted Successfully";
            return RedirectToAction("Index");


        }
    }
}
