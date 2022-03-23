using AspEndpoint.Models;

namespace AspEndpoint.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }
        public DbSet<FileModel> Files { get; set; } = null!;
        public DbSet<UserModel> Users { get; set; } = null!;
      
    }

}
