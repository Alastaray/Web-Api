using AspEndpoint.Models;
using AspEndpoint.Services;
using FileManagerLibrary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspEndpoint.Controllers
{
    [ApiController]  

    public class FileController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly IFileManager _fileManager;
        public FileController(DataContext context, IFileManager fileManager)
        {
            _dataContext = context;
            _fileManager = fileManager;
        }

        [Authorize]
        [HttpPost]
        [Route("api/upload-by-url")]
        public async Task<IActionResult> Upload([FromBody] UrlRequest link)
        {
            try
            {
                string host = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host + "/";
                FileDownloadService fileDownloadService = new FileDownloadService(_dataContext, _fileManager);
                string filePath = await fileDownloadService.FileDownloadAsync(link.Url);
                return this.JsonOk(host + filePath);
            }
            catch (ControllerExpection er)
            {
                return this.CreateJson(er.Message, er.StatusCode);
            }
            catch (Exception er)
            {
                return this.JsonBadRequest(er.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("api/get-url/:{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                string host = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host + "/";
                var fileModel = await _dataContext.Files.FindNotDeletedAsync(id);
                return this.JsonOk(host + fileModel.Path + fileModel.Name);
            }
            catch (ControllerExpection er)
            {
                return this.CreateJson(er.Message, er.StatusCode);
            }
            catch (Exception er)
            {
                return this.JsonBadRequest(er.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("api/remove/:{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            try
            {
                FileRemoveServise fileRemoveServise = new FileRemoveServise(_dataContext);
                return this.JsonOk(await fileRemoveServise.Remove(id));
            }
            catch (ControllerExpection er)
            {
                return this.CreateJson(er.Message, er.StatusCode);
            }
            catch (Exception er)
            {
                return this.JsonBadRequest(er.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("api/restore/:{id}")]
        public async Task<IActionResult> Restore(int id)
        {
            try
            {
                FileRemoveServise fileRemoveServise = new FileRemoveServise(_dataContext);
                return this.JsonOk(await fileRemoveServise.Restore(id));
            }
            catch (ControllerExpection er)
            {
                return this.CreateJson(er.Message, er.StatusCode);
            }
            catch (Exception er)
            {
                return this.JsonBadRequest(er.Message);
            }
        }


    }
}
