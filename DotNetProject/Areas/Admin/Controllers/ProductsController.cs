using System.Data.Common;
using System.IO;
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
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IUnitOfWork _db;
        public ProductsController(IUnitOfWork db, IWebHostEnvironment hostEnvironment)
        {
            _db = db;
            _hostEnvironment = hostEnvironment;
        }
        // GET: ProductsController
        public ActionResult Index()
        {
            List<Products> objList = _db.Product.GetAll(includeProperties: "Category").ToList();

            return View(objList);
        }
        public IActionResult Upsert(int? id)
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
            if (id == null || id == 0)
            {
                //create product
                return View(productVM);
            }
            else
            {
                //update product
                productVM.Products = _db.Product.Get(u => u.Id == id);
                return View(productVM);
            }

        }


        [HttpPost]
        public IActionResult Upsert(ProductVM productObj, IFormFile? file)
        {
            if (productObj.Products.Title == productObj.ToString())
            {
                ModelState.AddModelError("title", "The display order cannot exactly match the title");
            }
            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, "Images", "Product");
                    Directory.CreateDirectory(uploads);
                    if (string.IsNullOrEmpty(productObj.Products.ImageUrl) == false)
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, productObj.Products.ImageUrl.TrimStart('/'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    var extension = Path.GetExtension(file.FileName);
                    var filePath = Path.Combine(uploads, fileName + extension);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    productObj.Products.ImageUrl = $"/Images/Product/{fileName}{extension}";
                }

                if (productObj.Products.Id > 0)
                {
                    _db.Product.Update(productObj.Products);
                    _db.SaveChanges();
                    TempData["success"] = "Product Updated Successfully";
                }
                else
                {
                    _db.Product.Add(productObj.Products);
                    _db.SaveChanges();
                    TempData["success"] = "Product Created Successfully";
                }

                return RedirectToAction("Index");

            }

            return View(productObj);

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

        // # Region API Calls
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Products> objList = _db.Product.GetAll(includeProperties: "Category").ToList();
            return Json(new { data = objList });
        }
        [HttpDelete]
        public IActionResult DeleteAPI(int? id)
        {
            var obj = _db.Product.Get(u => u.Id == id);
            if (obj == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            var oldImagePath = Path.Combine(_hostEnvironment.WebRootPath, obj.ImageUrl.TrimStart('/'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            _db.Product.Remove(obj);
            _db.SaveChanges();
            return Json(new { success = true, message = "Delete Successful" });
        }

    //   # End call
    }
}

