
using Storage.Net;
using Storage.Net.Blobs;
namespace AspEndpoint.Services
{
    public class FileService
    {
        protected readonly FileContext _fileContext;
        protected readonly IConfiguration _config;
        protected readonly IBlobStorage? _storage;
        public FileService(FileContext context, IConfiguration configuration, bool setStorage = true)
        {
            _fileContext = context;
            _config = configuration;
            if (setStorage)
            {
                StorageFactory.Modules.UseGoogleCloudStorage();
                _storage = StorageFactory.Blobs.FromConnectionString(configuration.GetConnectionString("StorageConnection"));
            }
               

        }
    }
}
