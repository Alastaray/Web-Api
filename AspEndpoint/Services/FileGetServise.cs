using AspEndpoint.Models;

namespace AspEndpoint.Services
{
    public class FileGetServise : FileService
    {
        public FileGetServise(FileContext context, IConfiguration configuration) : base(context, configuration) { }
        public async Task<FileModel> GetImageAsync(int id)
        {
            var image = await _fileContext.files.FindAsync(id);
            if (image == null) throw new Exception("Record doesnot found!");
            return image;
        }
    }
}
