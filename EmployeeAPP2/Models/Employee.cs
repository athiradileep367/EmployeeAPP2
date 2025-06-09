using System;
using System.ComponentModel.DataAnnotations;

namespace EmployeeAPP2.Models
{
    public class Employee
    {
        public int Id { get; set; }
       
        [Required(ErrorMessage = "First Name is required")]
        [RegularExpression("^[a-zA-Z ]+$", ErrorMessage = "First Name must contain only letters")]
        public string FirstName { get; set; }
        [RegularExpression("^[a-zA-Z ]+$", ErrorMessage = "MiddleName must contain only letters")] 
        public string MiddleName { get; set; }

        [RegularExpression("^[a-zA-Z ]+$", ErrorMessage = "Last Name must contain only letters")]
        
        [Required(ErrorMessage = "Last Name is required")]
        
        public string LastName { get; set; }

        [Required(ErrorMessage = "Date of Birth is required")]
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email address")] 
        public string Email { get; set; }
        [Phone(ErrorMessage = "Invalid phone number")]
        [Required(ErrorMessage = "Mobile Number is required")]
        [StringLength(10, ErrorMessage = "Mobile number cannot exceed 10 digits.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Mobile number must be exactly 10 digits.")]
        public string MobileNumber { get; set; }

        [Required(ErrorMessage = "StreetAddress is required")] 
        public string StreetAddress { get; set; }
        [Required(ErrorMessage = "City is required")]
        [RegularExpression("^[a-zA-Z ]+$", ErrorMessage = "City Name must contain only letters")]
        public string City { get; set; }
        [Required(ErrorMessage = "State is required")]
        [RegularExpression("^[a-zA-Z ]+$", ErrorMessage = "State Name must contain only letters")]
        public string State { get; set; }
        [Required(ErrorMessage = "Country is required")]
        [RegularExpression("^[a-zA-Z ]+$", ErrorMessage = "Country Name must contain only letters")]
        public string Country { get; set; }


        [Required(ErrorMessage = "Zipcode is required")] 
        public string ZipCode { get; set; }
    
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
    }
}