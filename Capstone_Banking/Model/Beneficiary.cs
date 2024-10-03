namespace Capstone_Banking.Model
{
    public class Beneficiary
    {
        public int Id { get; set; }
        public string BenificiaryName { get; set; }
        public string Email { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsActive    { get; set; }
        public AccountDetails AccountDetailsObject { get; set; }
        public ICollection<Payment>? PaymentsList { get; set; }

        


    }
}
