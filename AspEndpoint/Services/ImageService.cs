namespace AspEndpoint.Services
{
    public class ImageService
    {
        protected readonly ImageContext _imageContext;
        protected readonly IConfiguration _config;
        public ImageService(ImageContext context, IConfiguration configuration)
        {
            _imageContext = context;
            _config = configuration;
        }
    }
}
