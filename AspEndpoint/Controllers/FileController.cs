using AspEndpoint.Models;
using AspEndpoint.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.Extensions.Configuration;
namespace AspEndpoint.Controllers
{
    [ApiController]  

    public class FileController : ControllerBase
    {
        private readonly ImageContext _imageContext;
        private readonly IConfiguration _config;
        public FileController(ImageContext context, IConfiguration configuration)
        {
            _imageContext = context;
            _config = configuration;
        }

        [HttpPost]
        [Route("api/upload-by-url")]
        public async Task<IActionResult> Upload([FromBody] UrlModel link)
        {
            try
            {
                string host = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host + "/";
                FileDownloadService fileDownloadService = new FileDownloadService(_imageContext, _config);
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
                FileGetServise fileGetServise = new FileGetServise(_imageContext, _config);
                var imageModel = await fileGetServise.GetImageAsync(id);
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
                FileRemoveServise fileRemoveServise = new FileRemoveServise(_imageContext, _config);
                return Ok(new MessageModel { Message = await fileRemoveServise.RemoveImage(id) });
            }
            catch (Exception er)
            {
                return BadRequest(new ErrorMessageModel { Error = er.Message });
            }

        }

        
       
    }
}
