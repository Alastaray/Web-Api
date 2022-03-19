using AspEndpoint.Models;
using AspEndpoint.Services;
using FileManagerLibrary;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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

        [HttpPost]
        [Route("api/upload-by-url")]
        public async Task<IActionResult> Upload([FromBody] RequestUrl link)
        {
            try
            {
                string host = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host + "/";
                FileDownloadService fileDownloadService = new FileDownloadService(_dataContext, _fileManager);
                string filePath = await fileDownloadService.FileDownloadAsync(link.Url);
                return Ok(new { Url = host + filePath });
            }
            catch (WebException)
            {
                return BadRequest(new { Error = "Url is incorrect!" });
            }
            catch (Exception er)
            {
                return BadRequest(new { Error = er.Message });
            }
        }

        [HttpGet]
        [Route("api/get-url/:{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                string host = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host + "/";
                FileGetServise fileGetServise = new FileGetServise(_dataContext);
                var imageModel = await fileGetServise.GetFileAsync(id);
                return Ok(new { Url = host + imageModel.Path + imageModel.Name });
            }
            catch (Exception er)
            {
                return BadRequest(new { Error = er.Message });
            }           
        }

        [HttpGet]
        [Route("api/remove/:{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            try
            {
                FileRemoveServise fileRemoveServise = new FileRemoveServise(_dataContext, _fileManager);
                return Ok(new { Message = await fileRemoveServise.RemoveImage(id) });
            }
            catch (Exception er)
            {
                return BadRequest(new { Error = er.Message });
            }

        }

        
       
    }
}
