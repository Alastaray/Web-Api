using AspEndpoint.Models;
using Microsoft.EntityFrameworkCore;

namespace AspEndpoint.Data
{
    public class ImageContext : DbContext
    {
        public ImageContext(DbContextOptions<ImageContext> options)
            : base(options)
        {
        }
        public DbSet<ImageModel> images { get; set; } = null!;
    }

}
