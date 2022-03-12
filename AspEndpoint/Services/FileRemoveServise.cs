using AspEndpoint.Models;

namespace AspEndpoint.Services
{
    public class FileRemoveServise : FileService
    {
        public FileRemoveServise(ImageContext context, IConfiguration configuration) : base(context, configuration) { }
        public async Task<string> RemoveImage(int id)
        {
            var imageModel = await new FileGetServise(_imageContext, _config).GetImageAsync(id);
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
                    File.Delete(image.Path + cut_size + "_" + image.Name);
            }
            File.Delete(image.Path + image.Name);
        }
    }
}
