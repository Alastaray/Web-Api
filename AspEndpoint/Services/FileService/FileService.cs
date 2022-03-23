
using FileManagerLibrary;
namespace AspEndpoint.Services
{
    public class FileService
    {
        protected readonly DataContext _dataContext;
        public FileService(DataContext context)
        {
            _dataContext = context;
        }
    }
}
