using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmployeeManagement.Controllers
{
    public class HomeController : Controller
    {
        EmpDBContext _context = new EmpDBContext();
        public ActionResult Index()
        {
            var listofData = _context.Employees.ToList();
            return View(listofData);
        }
        [HttpGet]

        public ActionResult Create()
        {
            return View();
        }
       [HttpPost]
        public ActionResult Create(Employee model)
        {
        try
        {
            if (_context.Employees.Any(e => e.EmpNo == model.EmpNo))
            {
                ModelState.AddModelError("EmpNo", "Employee Number must be unique. Duplicate Employee found.");
            }
            if (_context.Employees.Any(e => e.FirstName == model.FirstName))
            {
                ModelState.AddModelError("FirstName", "Duplicate firstname and lastname combination found.");
            }
            if (model.FirstName.Length > 15 || model.LastName.Length > 15)
            {
                ModelState.AddModelError("FirstName", "First name and Last name must be limited to 15 characters");
            }
            if (model.ContactNo.Length != 11 || !model.ContactNo.StartsWith("09"))
            {
                ModelState.AddModelError("ContactNo", "Contact Number must be 11 digits");
            }

            if (ModelState.IsValid)
            {
            _context.Employees.Add(model);
            _context.SaveChanges();
            ViewBag.Message = "Data Insert Successfully";
            return RedirectToAction("Index"); // Redirect to the Index action
            }

        }
            catch (Exception ex)
          {
        ViewBag.Message = $"Error: {ex.Message}";

        if (ex.InnerException != null)
        {
            ViewBag.InnerErrorMessage = $"Inner Exception: {ex.InnerException.Message}";
        }
          }

    return View(model);
}

        [HttpGet]

        public ActionResult Edit(int id)
        {
            var data = _context.Employees.Where(x => x.Id == id).FirstOrDefault();
            return View(data);
        }
        [HttpPost]

        public ActionResult Edit(Employee Model)
        {
            var data = _context.Employees.Where(x => x.Id == Model.Id).FirstOrDefault();
            if (data != null)
            {
                data.EmpNo = Model.EmpNo;
                data.FirstName = Model.FirstName;
                data.LastName = Model.LastName;
                data.Birthdate = Model.Birthdate;
                data.ContactNo = Model.ContactNo;
                data.EmailAddress = Model.EmailAddress;
                _context.SaveChanges();
            }
            return RedirectToAction("index");
        }

        public ActionResult Detail(int id)
        {
            var data = _context.Employees.Where(x => x.Id == id).FirstOrDefault();
            return View(data);
        }

        public ActionResult Delete(int id)
        {
            var data = _context.Employees.Where(x => x.Id == id).FirstOrDefault();
            _context.Employees.Remove(data);
            _context.SaveChanges();
            ViewBag.Message = "Record Delete Successfully";
            return RedirectToAction("index");
        }
    }
}
