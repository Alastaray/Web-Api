

using System.ComponentModel.DataAnnotations;

namespace AspEndpoint.Models
{
    public class RequestUrl
    {
        [Required]
        [Url]
        public string Url { get; set; } = String.Empty;
    }
}
