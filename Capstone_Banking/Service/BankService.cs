using Capstone_Banking.Dto;
using Capstone_Banking.Model;
using Capstone_Banking.Repository;

namespace Capstone_Banking.Service
{
    public class BankService : IBankService
    {
        private IBankRepository _bankRepository;

        public BankService(IBankRepository bankRepository)
        {
            _bankRepository = bankRepository;
        }
        public async Task AddBank(User user)
        {
            _bankRepository.AddBank(user);
        }

        public async Task<ICollection<Client>> GetAllClients()
        {
            return await _bankRepository.GetAllClients();
        }

        public async Task<Client> GetClientsById(int clientId)
        {
            return await _bankRepository.GetClientsById(clientId);
        }

        public async Task<ICollection<Documents>> GetDocuments(int clientId)
        {
            return await (_bankRepository.GetDocuments(clientId));
        }
        public async Task<ICollection<SalaryDisbursementResponseDto>> GetSalaryDisbursementClient(int clientId)
        {
            return await _bankRepository.GetSalaryDisbursementClient(clientId);
        }




    }
}
