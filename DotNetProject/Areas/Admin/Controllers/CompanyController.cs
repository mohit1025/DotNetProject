using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.DataAccess.Repository;
using Web.Models.Models;
using Web.Utilities;

namespace DotNetProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CompanyController : Controller
    {
        
        private readonly IUnitOfWork _db;
        public CompanyController(IUnitOfWork db)
        {
            _db= db;
        }

        // GET: CompanyController
        public ActionResult Index()
        {
           List<Company> objCompanyList = _db.Company.GetAll().ToList();
            return View(objCompanyList);
        }
         public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Company companyObj)
        {
            if (companyObj.Name == companyObj.ToString())
            {
                ModelState.AddModelError("name", "The display order cannot exactly match the name");
            }
            if (ModelState.IsValid)
            {
                _db.Company.Add(companyObj);
                _db.SaveChanges();
                TempData["success"] = "Company Created Succesfully";
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

            Company? company =  _db.Company.Get(u => u.Id == id);
            if (company == null)
            {
                return NotFound();
            }


            return View(company);
        }

        [HttpPost]
        public IActionResult Edit(Company companyObj)
        {
            if (ModelState.IsValid)
            {
                _db.Company.Update(companyObj);
                _db.SaveChanges();
                TempData["success"] = "Company Updated Succesfully";
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

            Company? company =  _db.Company.Get(u => u.Id ==id);
            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }
            Company? companyObj =  _db.Company.Get(u=> u.Id==id);
            if (companyObj == null)
            {
                return NotFound();
            }
            _db.Company.Remove(companyObj);
            _db.SaveChanges();
            TempData["success"] = "Company Deleted Succesfully";
            return RedirectToAction("Index");


        }
    }
}
