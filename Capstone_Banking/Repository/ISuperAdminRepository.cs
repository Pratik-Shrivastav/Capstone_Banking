using Capstone_Banking.Dto;
using Capstone_Banking.Model;

namespace Capstone_Banking.Repository
{
    public interface ISuperAdminRepository
    {
        public Task<ICollection<Client>> GetAllClientsPaged(int page, int pageSize);
        public Task<ICollection<User>> GetAllClientsPagedPending(int page, int pageSize);

        public ICollection<Client> GetAllClients();
        public  Task<int> GetClientCount(string status);

        public Task<Client> GetClientById(int id);
        public (ICollection<User>, int count) GetClientName(string companyName, string status, int page, int pageSize);


        public Task<ICollection<Documents>> GetDocuments(int clientId);

        public Task<(ICollection<SalaryDisbursementResponseDto>, int count)> GetSalaryDisbursementClient(int clientId, int page, int pageSize);

        public void ClientStatus(int clientId, string status);

        public void UpdatePaymentStatus(int clientId,int beneficiaryId, int payementId, string status);

        public void UpdateSalaryDisbursementStatus(int clientId,int salaryDisId, string status);

        public (ICollection<Beneficiary>, int count) BeneficiartyOption(int clientId, int page, int pageSize);

        public  (ICollection<Payment>, int count) PaymentsOfBeneficiary(int beneficiaryId, int page, int pageSize);

        public (ICollection<Beneficiary>, int count) GetBeneficiaryByName(int clientId, string beneficiaryName, int page, int pageSize);
        public (ICollection<Payment>, int count) GetPaymentByName(int beneficiaryId, string paymentName, int page, int pageSize);

    }
}
