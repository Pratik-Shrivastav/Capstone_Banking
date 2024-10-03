using Capstone_Banking.Dto;
using Capstone_Banking.Model;

namespace Capstone_Banking.Repository
{
    public interface IBankRepository
    {
        public void AddBank(User user);
        public Task<ICollection<Client>> GetAllClients();

        public Task<Client> GetClientsById(int clientId);


        public Task<ICollection<Documents>> GetDocuments(int clientId);

        public Task<ICollection<SalaryDisbursementResponseDto>> GetSalaryDisbursementClient(int clientId);




    }
}
