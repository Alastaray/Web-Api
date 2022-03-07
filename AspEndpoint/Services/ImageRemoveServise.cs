using AspEndpoint.Models;

namespace AspEndpoint.Services
{
    public class ImageRemoveServise
    {
        private readonly ImageContext _imageContext;
        public ImageRemoveServise(ImageContext context)
        {
            _imageContext = context;
        }
        public async Task<string> RemoveImage(int id)
        {
            var imageModel = await new ImageGetServise(_imageContext).GetImageAsync(id);
            DeleteFiles(imageModel);
            _imageContext.images.Remove(imageModel);
            await _imageContext.SaveChangesAsync();
            return "Successfully deleting!";
        }
        public void DeleteFiles(ImageModel image)
        {
            if (image.CutSizes != null)
            {
                string[] cut_sizes = image.CutSizes.Split('x');
                foreach (string cut_size in cut_sizes)
                    System.IO.File.Delete(image.Path + cut_size + "_" + image.Name);
            }
            File.Delete(image.Path + image.Name);
        }
    }
}
