using System;
using System.ComponentModel.DataAnnotations;
using Capstone_Banking.Model;

public class Client
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Founder Name is required.")]
    [StringLength(100, ErrorMessage = "Founder Name must be at most 100 characters long.")]
    public string FounderName { get; set; }

    [Required(ErrorMessage = "Company Name is required.")]
    [StringLength(100, ErrorMessage = "Company Name must be at most 100 characters long.")]
    public string CompanyName { get; set; }

    [EmailAddress(ErrorMessage = "Invalid email format.")]
    [StringLength(200, ErrorMessage = "Email must be at most 200 characters long.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Address is required.")]
    [StringLength(300, ErrorMessage = "Address must be at most 300 characters long.")]
    public string Address { get; set; }

    [Required(ErrorMessage = "City is required.")]
    [StringLength(100, ErrorMessage = "City must be at most 100 characters long.")]
    public string City { get; set; }

    [StringLength(100, ErrorMessage = "Region must be at most 100 characters long.")]
    public string Region { get; set; }

    [StringLength(20, ErrorMessage = "Postal Code must be at most 20 characters long.")]
    public string PostalCode { get; set; }

    [Required(ErrorMessage = "Country is required.")]
    [StringLength(100, ErrorMessage = "Country must be at most 100 characters long.")]
    public string Country { get; set; }

    [Phone(ErrorMessage = "Invalid phone number format.")]
    public string Phone { get; set; }

    [StringLength(50, ErrorMessage = "Status must be at most 50 characters long.")]
    public string Status { get; set; }

    public bool IsActive { get; set; }

    [Required(ErrorMessage = "Account details are required.")]
    public AccountDetails AccountDetailsObject { get; set; }

    public ICollection<Employee>? EmployeeList { get; set; } = new List<Employee>();
    public ICollection<Documents>? DocumentList { get; set; } = new List<Documents>();
    public ICollection<Beneficiary>? BeneficiaryList { get; set; } = new List<Beneficiary>();
    public ICollection<SalaryDisbursement>? SalaryDisbursementList { get; set; }

    public int? ForSalary {  get; set; }
    public int? ForPayment { get; set; }

}

