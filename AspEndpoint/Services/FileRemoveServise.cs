using AspEndpoint.Models;

namespace AspEndpoint.Services
{
    public class FileRemoveServise : FileService
    {
        public FileRemoveServise(FileContext context, IConfiguration configuration) : base(context, configuration) { }
        public async Task<string> RemoveImage(int id)
        {
            var imageModel = await new FileGetServise(_fileContext, _config).GetImageAsync(id);
            DeleteFiles(imageModel);
            _fileContext.files.Remove(imageModel);
            await _fileContext.SaveChangesAsync();
            return "Successfully deleting!";
        }
        public void DeleteFiles(FileModel image)
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
