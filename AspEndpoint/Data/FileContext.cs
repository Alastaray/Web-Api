using AspEndpoint.Models;
using Microsoft.EntityFrameworkCore;

namespace AspEndpoint.Data
{
    public class FileContext : DbContext
    {
        public FileContext(DbContextOptions<FileContext> options)
            : base(options)
        {
        }
        public DbSet<FileModel> files { get; set; } = null!;
    }

}
