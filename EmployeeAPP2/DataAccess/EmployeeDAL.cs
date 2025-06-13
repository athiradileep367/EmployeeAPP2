using EmployeeAPP2.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace EmployeeAPP2.DataAccess
{
    public class EmployeeDAL
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        //  Get All Employees
        public List<Employee> GetAllEmployees()// instance of the employee class
        {
            List<Employee> employees = new List<Employee>();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("GetAllEmployees", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();

                    SqlDataReader reader = cmd.ExecuteReader();// reads through every row only once
                    while (reader.Read())// returns true if the reader exists
                    {
                        employees.Add(new Employee
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            FirstName = reader["FirstName"].ToString(),
                            MiddleName = reader["MiddleName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]),
                            Email = reader["Email"].ToString(),
                            MobileNumber = reader["MobileNumber"].ToString(),
                            StreetAddress = reader["StreetAddress"].ToString(),
                            City = reader["City"].ToString(),
                            State = reader["State"].ToString(),
                            Country = reader["Country"].ToString(),
                            ZipCode = reader["ZipCode"].ToString(),
                            DepartmentId = Convert.ToInt32(reader["DepartmentId"]),
                            DepartmentName = reader["DepartmentName"].ToString()
                        });
                    }
                    reader.Close();
                }

                return employees;
            }
            catch (SqlException ex)
            {
                throw new Exception("Database Error while getting employee ", ex);
            }
            catch (Exception ex1)
            {
                throw new Exception(" An Unexpected Error", ex1);
            }
        }


        public Employee GetEmployeeById(int id)
        {
            Employee employee = null;
            
            using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("GetEmployeeById", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@EmployeeId", id);

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        employee = new Employee
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            FirstName = reader["FirstName"].ToString(),
                            MiddleName = reader["MiddleName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]),
                            Email = reader["Email"].ToString(),
                            MobileNumber = reader["MobileNumber"].ToString(),
                            StreetAddress = reader["StreetAddress"].ToString(),
                            City = reader["City"].ToString(),
                            State = reader["State"].ToString(),
                            Country = reader["Country"].ToString(),
                            ZipCode = reader["ZipCode"].ToString(),
                            DepartmentId = Convert.ToInt32(reader["DepartmentId"]),
                            DepartmentName = reader["DepartmentName"].ToString()
                        };
                    }

                    reader.Close();
                }
                return employee;
            
          
           
        }

        //  Add New Employee

        public void AddEmployee(Employee emp)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("AddEmployee", con); // NEW JSON-based procedure
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Create a JSON object from the Employee
                    var employeeJson = new
                    {
                        FirstName = emp.FirstName,
                        MiddleName = string.IsNullOrWhiteSpace(emp.MiddleName) ? null : emp.MiddleName,
                        LastName = emp.LastName,
                        DateOfBirth = emp.DateOfBirth?.ToString("yyyy-MM-dd"), // Format as string if not null
                        Email = emp.Email,
                        MobileNumber = emp.MobileNumber,
                        StreetAddress = emp.StreetAddress,
                        City = emp.City,
                        State = emp.State,
                        Country = emp.Country,
                        ZipCode = emp.ZipCode,
                        DepartmentId = emp.DepartmentId
                    };

                    string jsonString = JsonConvert.SerializeObject(employeeJson);

                    cmd.Parameters.AddWithValue("@json", jsonString);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            
            }
            catch (SqlException ex)
            {
                throw new Exception("Database Error while adding employee", ex);
            }
            catch (Exception ex1)
            {
                throw new Exception(" An Unexpected Error", ex1);
            }
        }
        

        // 🟢 Delete Employee
        public void DeleteEmployee(int Id)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("DeleteEmployee", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Pass JSON input
                    var json = JsonConvert.SerializeObject(new { Id = Id });
                    cmd.Parameters.AddWithValue("@json", json);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch(SqlException ex)
            {
                throw new Exception("Database Error while deleting", ex);
            }
            catch(Exception ex1)
            { 
                throw new Exception("Unexpected error", ex1);
            }
        }

        // 🟢 Update Employee
        public void UpdateEmployee(Employee emp)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("UpdateEmployee", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        var employeeJson = new
                        {
                            emp.Id,
                            emp.FirstName,
                            MiddleName = string.IsNullOrWhiteSpace(emp.MiddleName) ? null : emp.MiddleName,
                            emp.LastName,
                            DateOfBirth = emp.DateOfBirth?.ToString("yyyy-MM-dd"),
                            emp.Email,
                            emp.MobileNumber,
                            emp.StreetAddress,
                            emp.City,
                            emp.State,
                            emp.Country,
                            emp.ZipCode,
                            emp.DepartmentId
                        };

                        string jsonString = JsonConvert.SerializeObject(employeeJson);
                        cmd.Parameters.AddWithValue("@json", jsonString);

                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            
        

            catch (SqlException ex)
            {
          
                throw new Exception("Database Error While Updating Employee", ex);
            }
            catch (Exception ex1)
            {
                throw new Exception(" An Unexpected Error", ex1);
            }
        }
    }

        
    
}
