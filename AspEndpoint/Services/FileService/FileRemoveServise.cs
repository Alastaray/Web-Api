using AspEndpoint.Models;
using FileManagerLibrary;
using Storage.Net.Blobs;
namespace AspEndpoint.Services
{
    public class FileRemoveServise : FileService
    {
        public FileRemoveServise(DataContext context) : base(context) { }

        public async Task<string> Remove(int id)
        {
            FileModel fileModel = await _dataContext.Files.FindNotDeletedAsync(id);
            _dataContext.Files.SoftDelete(fileModel);
            await _dataContext.SaveChangesAsync();
            return "Record successfully deleted!";
        }
        public async Task<string> Restore(int id)
        {
            FileModel fileModel = await _dataContext.Files.FindDeletedAsync(id);           
            _dataContext.Files.Restore(fileModel);
            await _dataContext.SaveChangesAsync();
            return "Record successfully restored!";
        }
    }
}
