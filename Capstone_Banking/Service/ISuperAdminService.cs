using Capstone_Banking.Dto;
using Capstone_Banking.Model;

namespace Capstone_Banking.Service
{
    public interface ISuperAdminService
    {
        public Task<(ICollection<Client>, int totalCount)> GetAllClientsPaged(int page, int pageSize);
        
       public Task<(ICollection<User>, int totalCount)> GetAllClientsPagedPending(int page, int pageSize);

        public Task<ICollection<Client>> GetAllClients();

        public Task<Client> GetClientsById(int id);
        public Task<(ICollection<User>, int count)> GetClientName(string companyName, string status, int page, int pageSize);
        public Task<ICollection<Documents>> GetDocumentById(int Clientd);

        public void UpdateClientStatus(int clientId, string status);

        public Task UpdatePaymentStatus(int clientId,int beneficiaryId,int payementId, string status);

        public Task<Object> GetSalaryDisbursementClient(int clientId, int page, int pageSize);

        public Task UpdateSalaryDisbursementStatus(int clientId,int salaryDisId, string status);

        public Object BeneficiartyOption(int clientId, int page, int pageSize);

        public Object PaymentsOfBeneficiary(int beneficiaryId, int page, int pageSize);





    }
}
