namespace Capstone_Banking.Model
{
    public class Bank
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ContactPerson { get; set; }
        public string Address { get; set; }
        public string ContactInfo { get; set; }
        public ICollection<Client>? ClientList { get; set; }
    }
}
