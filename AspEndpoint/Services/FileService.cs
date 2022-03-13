

namespace AspEndpoint.Services
{
    public class FileService
    {
        protected readonly FileContext _fileContext;
        protected readonly IConfiguration _config;
        public FileService(FileContext context, IConfiguration configuration)
        {
            _fileContext = context;
            _config = configuration;
            
        }
    }
}
