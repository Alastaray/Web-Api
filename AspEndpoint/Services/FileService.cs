
using FileManagerProject;
using Storage.Net;
using Storage.Net.Blobs;
namespace AspEndpoint.Services
{
    public class FileService
    {
        protected readonly FileContext _fileContext;
        protected readonly IFileManager _fileManager;
        public FileService(FileContext context, IFileManager fileManager)
        {
            _fileContext = context;
            _fileManager = fileManager;
        }
    }
}
