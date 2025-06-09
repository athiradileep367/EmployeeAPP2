using EmployeeAPP2.Models;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace EmployeeAPP2.DataAccess
{
    public class DepartmentDAL
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public List<Department> GetAllDepartments()
        {
            List<Department> departments = new List<Department>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetAllDepartments", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Department dept = new Department
                    {
                        
                        DepartmentID = (int)reader["DepartmentID"],
                        DepartmentName = reader["DepartmentName"].ToString()
                    };
                    
                    departments.Add(dept);
                }
            }

            return departments;
        }
    }
}
