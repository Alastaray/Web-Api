using AspEndpoint.Models;

namespace AspEndpoint.Services
{
    public class ImageGetServise
    {
        private readonly ImageContext _imageContext;
        public ImageGetServise(ImageContext context)
        {
            _imageContext = context;
        }
        public async Task<ImageModel> GetImageAsync(int id)
        {
            var image = await _imageContext.images.FindAsync(id);
            if (image == null) throw new Exception("Record doesnot found!");
            return image;
        }
    }
}
