using System.ComponentModel.DataAnnotations;

namespace AspEndpoint.Requests
{
    public class UrlRequest
    {
        [Required]
        [Url]
        public string Url { get; set; } = String.Empty;
    }
}
