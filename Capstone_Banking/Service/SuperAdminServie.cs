using Capstone_Banking.Dto;
using Capstone_Banking.Model;
using Capstone_Banking.Repository;

namespace Capstone_Banking.Service
{
    public class SuperAdminServie : ISuperAdminService
    {
        private ISuperAdminRepository _superAdminRepository;

        public SuperAdminServie(ISuperAdminRepository superAdminRepo)
        {
            _superAdminRepository = superAdminRepo;
        }

        public async Task<(ICollection<Client>, int totalCount)> GetAllClientsPaged(int page, int pageSize)
        {
            var clients = await _superAdminRepository.GetAllClientsPaged(page, pageSize);
            int totalClients = await _superAdminRepository.GetClientCount("Success");
            return (clients,totalClients );
        }
        public async Task<(ICollection<User>, int totalCount)> GetAllClientsPagedPending(int page, int pageSize)
        {
            var clients = await _superAdminRepository.GetAllClientsPagedPending(page, pageSize);
            int totalClients = await _superAdminRepository.GetClientCount("Pending");
            return (clients, totalClients);
        }

        public async Task<ICollection<Client>> GetAllClients()
        {
            return _superAdminRepository.GetAllClients();
        }


        public async Task<Client> GetClientsById(int id)
        {
           return await _superAdminRepository.GetClientById(id);
        }

        public async Task<(ICollection<User>, int count)> GetClientName(string companyName, string status, int page, int pageSize)
        {
            var (paginatedUser, count) =  _superAdminRepository.GetClientName(companyName,status,page,pageSize);
            
            return (paginatedUser, count);
        }

        public Task<ICollection<Documents>> GetDocumentById(int clientId)
        {
            return _superAdminRepository.GetDocuments(clientId);
        }

        public void UpdateClientStatus(int clientId, string status)
        {
            _superAdminRepository.ClientStatus(clientId, status);
        }

        public async Task UpdatePaymentStatus(int clientId,int beneficiaryId, int payementId, string status)
        {
            _superAdminRepository.UpdatePaymentStatus(clientId, beneficiaryId,payementId, status);
        }

        public async Task<Object> GetSalaryDisbursementClient(int clientId, int page, int pageSize)
        {
            var (paginatedSalary, count)= await _superAdminRepository.GetSalaryDisbursementClient(clientId, page, pageSize);
            return new {paginatedSalary, count};
        }

        public async Task UpdateSalaryDisbursementStatus(int clientId,int salaryDisId, string status)
        {
            _superAdminRepository.UpdateSalaryDisbursementStatus(clientId,salaryDisId, status);
        }



    }
}
