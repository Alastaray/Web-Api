using AspEndpoint.Models;

namespace AspEndpoint.Services
{
    public class FileGetServise : FileService
    {
        public FileGetServise(FileContext context, IConfiguration configuration) : base(context, configuration, false) { }
        public async Task<FileModel> GetFileAsync(int id)
        {
            var image = await _fileContext.files.FindAsync(id);
            if (image == null) throw new Exception("Record doesnot found!");
            return image;
        }
    }
}
