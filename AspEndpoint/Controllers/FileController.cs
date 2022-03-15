using AspEndpoint.Models;
using AspEndpoint.Services;
using FileManagerProject;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace AspEndpoint.Controllers
{
    [ApiController]  

    public class FileController : ControllerBase
    {
        private readonly FileContext _fileContext;
        private readonly IFileManager _fileManager;
        public FileController(FileContext context, IFileManager fileManager)
        {
            _fileContext = context;
            _fileManager = fileManager;
        }

        [HttpPost]
        [Route("api/upload-by-url")]
        public async Task<IActionResult> Upload([FromBody] UrlModel link)
        {
            try
            {
                string host = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host + "/";
                FileDownloadService fileDownloadService = new FileDownloadService(_fileContext, _fileManager);
                string filePath = await fileDownloadService.FileDownloadAsync(link.Url);
                return Ok(new UrlModel { Url = host + filePath });
            }
            catch (WebException)
            {
                return BadRequest(new ErrorMessageModel { Error = "Url is incorrect!" });
            }
            catch (Exception er)
            {
                return BadRequest(new ErrorMessageModel { Error = er.Message });
            }
        }

        [HttpGet]
        [Route("api/get-url/:{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                string host = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host + "/";
                FileGetServise fileGetServise = new FileGetServise(_fileContext);
                var imageModel = await fileGetServise.GetFileAsync(id);
                return Ok(new UrlModel { Url = host + imageModel.Path + imageModel.Name });
            }
            catch (Exception er)
            {
                return BadRequest(new ErrorMessageModel { Error = er.Message });
            }           
        }

        [HttpGet]
        [Route("api/remove/:{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            try
            {
                FileRemoveServise fileRemoveServise = new FileRemoveServise(_fileContext, _fileManager);
                return Ok(new MessageModel { Message = await fileRemoveServise.RemoveImage(id) });
            }
            catch (Exception er)
            {
                return BadRequest(new ErrorMessageModel { Error = er.Message });
            }

        }

        
       
    }
}
