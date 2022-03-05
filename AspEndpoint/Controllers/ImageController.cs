using AspEndpoint.Models;
using AspEndpoint.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace AspEndpoint.Controllers
{
    [ApiController]  

    public class ImageController : ControllerBase
    {
        private readonly ImageContext context;
        public ImageController(ImageContext _context)
        {
            context = _context;
        }

        [HttpPost]
        [Route("api/upload-by-url")]
        public async Task<IActionResult> UploadByUrl(string url)
        {
            try
            {
                ImageDownloadService imageDownloadService = new ImageDownloadService(context);
                string host = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host + "/";
                return Ok(new LinkModel { Url = host + await imageDownloadService.DownloadImageAsync(url)});
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
        [Route("api/get-url")]
        public async Task<IActionResult> GetUrl(int id)
        {
            string host = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host + "/";
            var image = await context.images.FindAsync(id);
            if (image == null) return NotFound(new ErrorMessageModel { Error = "Record doesnot found!" });
            return Ok(new LinkModel { Url = host + image.Path + image.Name });
        }

        [HttpGet]
        [Route("api/remove")]
        public async Task<IActionResult> RemoveImage(int id)
        {
            try
            {
                ImageRemoveServise imageRemoveServise = new ImageRemoveServise(context);
                return Ok(new MessageModel { message = await imageRemoveServise.RemoveImage(id) });
            }
            catch (Exception er)
            {
                return BadRequest(new ErrorMessageModel { Error = er.Message });
            }

        }

        
       
    }
}
