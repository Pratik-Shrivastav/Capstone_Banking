using Capstone_Banking.Dto;

namespace Capstone_Banking.Service
{
    public interface ISuperAdminService
    {
        public Task<ICollection<Client>> GetAllClients();
        public Task<Client> GetClientsById(int id);

        public Task<ICollection<Documents>> GetDocumentById(int Clientd);

        public void UpdateClientStatus(int clientId, string status);

        public Task UpdatePaymentStatus(int clientId,int payementId, string status);

        public Task<ICollection<SalaryDisbursementResponseDto>> GetSalaryDisbursementClient(int clientId);

        public Task UpdateSalaryDisbursementStatus(int clientId,int salaryDisId, string status);



    }
}
