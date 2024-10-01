using System;
using Capstone_Banking.Model;

public class Client
{
        public int Id { get; set; }
        public string FounderName { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Status { get; set; }
        public bool IsActive { get; set; }
        public AccountDetails AccountDetailsObject { get; set; }
        public ICollection<Employee>? EmployeeList { get; set; }
        public ICollection<Documents>? DocumentList { get; set; } = new List<Documents>();
        public ICollection<Payment>? Payments { get; set; }

}

