using System.ComponentModel.DataAnnotations;
namespace AspEndpoint.Models
{
    public class UserRequest
    {
        [Required]
        public string? Login { get; set; } = string.Empty;
        [Required]
        public string? Password { get; set; } = string.Empty;
    }
}
