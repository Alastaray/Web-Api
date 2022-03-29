namespace AspEndpoint.Models
{
    public class RefreshSessionModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public UserModel? User { get; set; }
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
