using AspEndpoint.Requests;
using AspEndpoint.Services.FileService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspEndpoint.Controllers
{
    [ApiController]  

    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;
        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [Authorize]
        [HttpPost]
        [Route("api/upload-by-url")]
        public async Task<IActionResult> Upload([FromBody] UrlRequest link)
        {
            try
            {              
                string filePath = await _fileService.FileDownloadAsync(link.Url);
                return this.JsonOk(GetHost() + filePath);
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
                var fileModel = await _fileService.GetAsync(id);
                return this.JsonOk(GetHost() + fileModel.Path + fileModel.Name);
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
                return this.JsonOk(await _fileService.Remove(id));
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
                return this.JsonOk(await _fileService.Restore(id));
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

        private string GetHost()
        {
            return HttpContext.Request.Scheme + "://" + HttpContext.Request.Host + "/";
        }

    }
}
