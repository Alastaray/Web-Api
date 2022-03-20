

using System.ComponentModel.DataAnnotations;

namespace AspEndpoint.Models
{
    public class UrlRequest
    {
        [Required]
        [Url]
        public string Url { get; set; } = String.Empty;
    }
}
