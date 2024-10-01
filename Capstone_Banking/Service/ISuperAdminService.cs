namespace Capstone_Banking.Service
{
    public interface ISuperAdminService
    {
        public Task<ICollection<Client>> GetAllClients();
        public Task<Client> GetClientsById(int id);

        public Task<ICollection<Documents>> GetDocumentById(int Clientd);

    }
}
