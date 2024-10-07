namespace Capstone_Banking.Dto
{
    public class RegisterDto
    {
        public string FounderName { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string UserName { get; set; }
        public string AccountNumber { get; set; }
        public string IFSC { get; set; }
        public string Branch { get; set; }
        public int? ForSalary { get; set; }
        public int? ForPayment { get; set; }
    }
}
