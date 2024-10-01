namespace Capstone_Banking.Repository
{
    public interface ISuperAdminRepository
    {
        public Task<ICollection<Client>> GetAllClients();

        public Task<Client> GetClientById(int id);

        public Task<ICollection<Documents>> GetDocuments(int clientId);

        public void ClientStatus(int clientId, string status);


    }
}
