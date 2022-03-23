
namespace AspEndpoint.Models
{
    public class FileModel: IDeletedAt
    {
        public int Id { get; set; }
        public string? Path { get; set; } = string.Empty;
        public string? Name { get; set; } = string.Empty;
        public DateTime? DeletedAt { get; set; } = null;
    }
}
