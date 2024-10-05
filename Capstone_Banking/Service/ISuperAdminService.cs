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
        public Task<ICollection<User>> GetClientName(string companyName, string status);
        public Task<ICollection<Documents>> GetDocumentById(int Clientd);

        public void UpdateClientStatus(int clientId, string status);

        public Task UpdatePaymentStatus(int clientId,int beneficiaryId,int payementId, string status);

        public Task<ICollection<SalaryDisbursementResponseDto>> GetSalaryDisbursementClient(int clientId);

        public Task UpdateSalaryDisbursementStatus(int clientId,int salaryDisId, string status);



    }
}
