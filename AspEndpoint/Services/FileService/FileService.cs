
using FileManagerLibrary;
namespace AspEndpoint.Services
{
    public class FileService
    {
        protected readonly DataContext _fileContext;
        protected readonly IFileManager _fileManager;
        public FileService(DataContext context, IFileManager fileManager)
        {
            _fileContext = context;
            _fileManager = fileManager;
        }
    }
}
