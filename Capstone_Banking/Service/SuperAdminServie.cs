using Capstone_Banking.Dto;
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
            int totalClients = await _superAdminRepository.GetClientCount();
            return (clients,totalClients );
        }

        public async Task<ICollection<Client>> GetAllClients()
        {
            return _superAdminRepository.GetAllClients();
        }


        public async Task<Client> GetClientsById(int id)
        {
           return await _superAdminRepository.GetClientById(id);
        }

        public async Task<ICollection<Client>> GetClientName(string companyName)
        {
            var clients = _superAdminRepository.GetClientName(companyName);
            if (clients.Count() == 0) 
            {
                return null;
            }
            return clients;
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

        public async Task<ICollection<SalaryDisbursementResponseDto>> GetSalaryDisbursementClient(int clientId)
        {
            return await _superAdminRepository.GetSalaryDisbursementClient(clientId);
        }

        public async Task UpdateSalaryDisbursementStatus(int clientId,int salaryDisId, string status)
        {
            _superAdminRepository.UpdateSalaryDisbursementStatus(clientId,salaryDisId, status);
        }



    }
}
