using Capstone_Banking.Dto;
using Capstone_Banking.Model;

namespace Capstone_Banking.Service
{
    public interface IBankService
    {
        public Task AddBank(User user);
        public Task<ICollection<Client>> GetAllClients();

        public Task<Client> GetClientsById(int clientId);
        public Task<ICollection<Documents>> GetDocuments(int clientId);
        public Task<ICollection<SalaryDisbursementResponseDto>> GetSalaryDisbursementClient(int clientId);


    }
}
