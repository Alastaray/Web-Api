using AspEndpoint.Models;

namespace AspEndpoint.Services
{
    public class FileGetServise : FileService
    {
        public FileGetServise(DataContext context) : base(context, null) { }
        public async Task<FileModel> GetFileAsync(int id)
        {
            var image = await _fileContext.files.FindAsync(id);
            if (image == null) throw new Exception("Record doesnot found!");
            return image;
        }
    }
}
