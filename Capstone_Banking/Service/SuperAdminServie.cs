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

        public async Task<ICollection<Client>> GetAllClients()
        {
            return await _superAdminRepository.GetAllClients();
        }

        public async Task<Client> GetClientsById(int id)
        {
           return await _superAdminRepository.GetClientById(id);
        }

        public Task<ICollection<Documents>> GetDocumentById(int clientId)
        {
            return _superAdminRepository.GetDocuments(clientId);
        }

        public void UpdateClientStatus(int clientId, string status)
        {
            _superAdminRepository.ClientStatus(clientId, status);
        }

        public async Task UpdatePaymentStatus(int clientId, int payementId, string status)
        {
            _superAdminRepository.UpdatePaymentStatus(clientId,payementId, status);
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
