using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace AspEndpoint.Models
{
    public class ImageContext : DbContext
    {
        public ImageContext(DbContextOptions<ImageContext> options)
            : base(options)
        {
        }
        public DbSet<ImageModel> TodoItems { get; set; } = null!;
    }

}
