using AspEndpoint.Models;
using FileManagerLibrary;
using Storage.Net.Blobs;
namespace AspEndpoint.Services
{
    public class FileRemoveServise : FileService
    {
        public FileRemoveServise(DataContext context, IFileManager fileManager) : base(context, fileManager) { }

        public async Task<string> RemoveImage(int id)
        {
            FileModel fileModel = await new FileGetServise(_fileContext).GetFileAsync(id);
            await _fileManager.RemoveFilesAsync(fileModel.Path);
            await _fileManager.RemoveSubFoldersAsync(fileModel.Path);
            _fileContext.files.Remove(fileModel);
            await _fileContext.SaveChangesAsync();
            return "Successfully deleting!";
        }       
    }
}
