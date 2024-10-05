namespace Capstone_Banking.Dto
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }
        public string? Status { get; set; }
        public int Id { get; set; }
        public string Role { get; set; }
        public string UserName { get; set; }
    }
}
