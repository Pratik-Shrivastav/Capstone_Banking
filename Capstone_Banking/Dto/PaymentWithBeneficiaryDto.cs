namespace Capstone_Banking.Dto
{
    public class PaymentWithBeneficiaryDto
    {
        public int Id { get; set; }
        public string PaymentType { get; set; }
        public double Amount { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public int BeneficiaryId { get; set; }
        public string BeneficiaryName { get; set; }
        public DateTime BeneficiaryCreatedOn { get; set; }
        public bool BeneficiaryIsActive { get; set; }
    }

}
