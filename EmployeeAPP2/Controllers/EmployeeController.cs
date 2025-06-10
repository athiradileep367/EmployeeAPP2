using EmployeeAPP2.DataAccess;
using EmployeeAPP2.Models;
using EmployeeAPP2.Models.ViewModels;
using System.Collections.Generic;
using System.Web.Mvc;
using System;

namespace EmployeeAPP2.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly EmployeeDAL employeeDAL = new EmployeeDAL();
        private readonly DepartmentDAL departmentDAL = new DepartmentDAL();

        // GET: /Employee/
       
            public ActionResult Index()
        {
            var employees = employeeDAL.GetAllEmployees();
            var departments = departmentDAL.GetAllDepartments();

            var viewModel = new EmployeeDepartmentViewModel
            {
                Employees = employees,
                Departments = departments
            };

            return View(viewModel);  // pass the ViewModel 
        }

        

        // Show the Add Employee modal form (GET)
        public ActionResult AddEmployee()
        {
            var model = new EmployeeDepartmentViewModel
            {
                Employee = new Employee(),                // Empty employee for create
                Departments = departmentDAL.GetAllDepartments()  // Load departments for dropdown
            };
            return PartialView("_AddEmployee", model);
        }

        // Save employee data (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Employee employee)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    if (employee.Id == 0)
                    {
                        employeeDAL.AddEmployee(employee);      // Add new employee
                    }
                    else
                    {
                        employeeDAL.UpdateEmployee(employee);   // Update existing employee
                    }
                    return Json(new { success = true });
                }
            }
            
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = (ex.Message);
            }
            

          
        // If validation fails, reload form with current data and departments
        var model = new EmployeeDepartmentViewModel
            {
                Employee = employee,
                Departments = departmentDAL.GetAllDepartments()
            };
            return PartialView("_AddEmployee", model);
        }


        public JsonResult GetEmployees()
        {
            var employees = employeeDAL.GetAllEmployees();
            return Json(new { data = employees }, JsonRequestBehavior.AllowGet);
        }




        // GET: /Employee/Edit/5
        public ActionResult EditEmployee(int id)
        {
            
                var employee = employeeDAL.GetEmployeeById(id);

                if (employee == null) return HttpNotFound();
                var model = new EmployeeDepartmentViewModel
                {
                    Employee = employee,
                    Departments = departmentDAL.GetAllDepartments()
                };
                return PartialView("_AddEmployee", model); // Reuse same modal
            
         
        }
            //var model = new EmployeeDepartmentViewModel
            //{
            //    Employee = employee,
            //    Departments = departmentDAL.GetAllDepartments()
            //};
            //return PartialView("_AddEmployee", model); // Reuse same modal
        
            //}

        // POST: /Employee/Update
        //[HttpPost]
        //public ActionResult Update(Employee emp)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            employeeDAL.UpdateEmployee(emp);
        //            return Json(new { success = true });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ViewBag.ErrorMessage = ("An error while updating employee " + ex.Message);
        //    }

        //    var model = new EmployeeDepartmentViewModel
        //    {
        //        Employee = emp,
        //        Departments = departmentDAL.GetAllDepartments()
        //    };
        //    return PartialView("_AddEmployee", model);
        //}

        // POST: /Employee/Delete/5
        [HttpPost]
        public ActionResult DeleteEmployee(int Id)
        {
           
                employeeDAL.DeleteEmployee(Id);
                return Json(new { success = true });
           
        }
    }
}
