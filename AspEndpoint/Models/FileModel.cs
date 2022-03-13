

namespace AspEndpoint.Models
{
    public class FileModel
    {
        public int Id { get; set; }
        public string? Path { get; set; } = string.Empty;
        public string? Name { get; set; } = string.Empty;
        public string? CutSizes { get; set; } = string.Empty;

    }
}
