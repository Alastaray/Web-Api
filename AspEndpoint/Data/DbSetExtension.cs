using AspEndpoint.Helpers;
using AspEndpoint.Models;

namespace AspEndpoint.Data
{
    public static class DbSetExtension
    {
        public static void SoftDelete<TEntity>(this DbSet<TEntity> dbset, TEntity entity) where TEntity : class, IDeletedAt
        {
            entity.DeletedAt = DateTime.UtcNow;
            dbset.Update(entity);
        }
        public static void Restore<TEntity>(this DbSet<TEntity> dbset, TEntity entity) where TEntity : class, IDeletedAt
        {
            entity.DeletedAt = null;
            dbset.Update(entity);
        }
        public static async ValueTask<TEntity> FindDeletedAsync<TEntity>(this DbSet<TEntity> dbset, int id) where TEntity : class, IDeletedAt
        {
            var obj = await FindAsync(dbset, id);
            if (obj.DeletedAt == null) throw new Exception("Record was not deleted!");
            return obj;
        }
        public static async ValueTask<TEntity> FindNotDeletedAsync<TEntity>(this DbSet<TEntity> dbset, int id) where TEntity : class, IDeletedAt
        {
            var obj = await FindAsync(dbset, id);
            if (obj.DeletedAt != null) throw new Exception("Record has already deleted!");
            return obj;
        }
        public static IQueryable<TEntity> GetAllDeleted<TEntity>(this DbSet<TEntity> dbset) where TEntity : class, IDeletedAt
        {
            return dbset.Where(x => x.DeletedAt != null);
        }
        public static IQueryable<TEntity> GetAllNotDeleted<TEntity>(this DbSet<TEntity> dbset) where TEntity : class, IDeletedAt
        {
            return dbset.Where(x => x.DeletedAt == null);
        }
        public static IEnumerable<TEntity> GetAllDeleted<TEntity>(this DbSet<TEntity> dbset, Func<TEntity, bool> predicate) where TEntity : class, IDeletedAt
        {
            var lyamda = (TEntity entity) => { return entity.DeletedAt != null && predicate.Invoke(entity); };
            return dbset.Where(lyamda);
        }
        public static IEnumerable<TEntity> GetAllNotDeleted<TEntity>(this DbSet<TEntity> dbset, Func<TEntity, bool> predicate) where TEntity : class, IDeletedAt
        {
            var lyamda = (TEntity entity) => { return entity.DeletedAt == null && predicate.Invoke(entity); };
            return dbset.Where(lyamda);
        }
        private static async ValueTask<TEntity> FindAsync<TEntity>(DbSet<TEntity> dbset, int id) where TEntity : class, IDeletedAt
        {
            return await dbset.FindAsync(id) ?? throw new ControllerExpection("Record was not found!", ResponseStatusCode.NotFound);
        }
    }
}
