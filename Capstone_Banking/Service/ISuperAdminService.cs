namespace Capstone_Banking.Service
{
    public interface ISuperAdminService
    {
        public Task<ICollection<Client>> GetAllClients();

    }
}
