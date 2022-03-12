namespace AspEndpoint.Services
{
    public class FileService
    {
        protected readonly ImageContext _imageContext;
        protected readonly IConfiguration _config;
        public FileService(ImageContext context, IConfiguration configuration)
        {
            _imageContext = context;
            _config = configuration;
        }
    }
}
