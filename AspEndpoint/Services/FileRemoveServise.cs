using AspEndpoint.Models;
using Storage.Net.Blobs;
namespace AspEndpoint.Services
{
    public class FileRemoveServise : FileService
    {
        public FileRemoveServise(FileContext context, IConfiguration configuration) : base(context, configuration) { }

        public async Task<string> RemoveImage(int id)
        {
            var fileModel = await new FileGetServise(_fileContext, _config).GetFileAsync(id);
            await RemoveFilesAsync(fileModel);
            _fileContext.files.Remove(fileModel);
            await _fileContext.SaveChangesAsync();
            return "Successfully deleting!";
        }
        private async Task RemoveFilesAsync(FileModel file)
        {
            var filesList = await _storage.ListAsync(file.Path);
            foreach (var fileModel in filesList)
                await _storage.DeleteAsync(fileModel);

            if(file.Path != null)
            {
                string[]? folders = file.Path.Split('/');
                foreach (var folder in folders)
                {
                    if(folder != "")
                        await _storage.DeleteAsync(folder);
                }                   
            }
            
        }
    }
}
