

using System.ComponentModel.DataAnnotations;

namespace AspEndpoint.Models
{
    public class UrlModel
    {
        [Required]
        [Url]
        public string Url { get; set; } = String.Empty;
    }
}
