namespace BookStore.BL.Auth.Entities
{
    public class LoginResponse
    {
        public bool Success { get; set; }
        public string? Token { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
