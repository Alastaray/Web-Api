using System.ComponentModel.DataAnnotations;
namespace AspEndpoint.Requests
{
    public class AuthorizationRequest
    {
        [Required]
        [MaxLength(30)]
        public string? Login { get; set; } = string.Empty;
        [Required]
        [MaxLength(30)]
        public string? Password { get; set; } = string.Empty;
    }
}
