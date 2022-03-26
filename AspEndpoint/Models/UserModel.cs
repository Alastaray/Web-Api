namespace AspEndpoint.Models
{
    public class UserModel : IDeletedAt
    {
        public int Id { get; set; }
        public string? Name { get; set; } = string.Empty;
        public string? Surname { get; set; } = string.Empty;
        public string? Login { get; set; } = string.Empty;
        public string? Password { get; set; } = string.Empty;
        public DateTime? CreatedAt { get; set; } = null;
        public DateTime? DeletedAt { get; set; } = null;      
        public string? RefreshToken { get; set; } = string.Empty;

    }
}
