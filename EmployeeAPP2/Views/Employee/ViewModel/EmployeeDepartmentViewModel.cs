using System.Collections.Generic;
using EmployeeAPP2.Models; 

namespace EmployeeAPP2.Models.ViewModels // subfolder inside models
{
    public class EmployeeDepartmentViewModel
    {
        public Employee Employee { get; set; }
        public List<Employee> Employees { get; set; }
        public List<Department> Departments { get; set; }
    }
}
