using System.ComponentModel.DataAnnotations;

namespace AspEndpoint.Requests
{
    public class RegistrationRequest
    {
        [Required]
        [MaxLength(30)]
        public string Name { get; set; } = string.Empty;
        [Required]
        [MaxLength(30)]
        public string Surname { get; set; } = string.Empty;
        [Required]
        [MaxLength(30)]
        public string Login { get; set; } = string.Empty;
        [Required]
        [MaxLength(30)]
        public string Password { get; set; } = string.Empty;
    }
}
