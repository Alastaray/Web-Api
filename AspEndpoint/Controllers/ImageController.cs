using AspEndpoint.Models;
using AspEndpoint.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.Extensions.Configuration;
namespace AspEndpoint.Controllers
{
    [ApiController]  

    public class ImageController : ControllerBase
    {
        private readonly ImageContext _imageContext;
        private readonly IConfiguration _config;
        public ImageController(ImageContext context, IConfiguration configuration)
        {
            _imageContext = context;
            _config = configuration;
        }

        [HttpPost]
        [Route("api/upload-by-url")]
        public async Task<IActionResult> UploadByUrl([FromBody] UrlModel link)
        {
            try
            {
                ImageDownloadService imageDownloadService = new ImageDownloadService(_imageContext, _config);
                string host = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host + "/";
                return Ok(new UrlModel { Url = host + await imageDownloadService.ProcessImageAsync(link.Url) });
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
        public async Task<IActionResult> GetUrl(int id)
        {
            try
            {
                string host = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host + "/";
                var imageModel = await new ImageGetServise(_imageContext, _config).GetImageAsync(id);
                return Ok(new UrlModel { Url = host + imageModel.Path + imageModel.Name });
            }
            catch (Exception er)
            {
                return BadRequest(new ErrorMessageModel { Error = er.Message });
            }           
        }

        [HttpGet]
        [Route("api/remove/:{id}")]
        public async Task<IActionResult> RemoveImage(int id)
        {
            try
            {
                ImageRemoveServise imageRemoveServise = new ImageRemoveServise(_imageContext, _config);
                return Ok(new MessageModel { Message = await imageRemoveServise.RemoveImage(id) });
            }
            catch (Exception er)
            {
                return BadRequest(new ErrorMessageModel { Error = er.Message });
            }

        }

        
       
    }
}
