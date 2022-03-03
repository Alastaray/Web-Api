using AspEndpoint.Models;
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
                if (GetPictureSize(url) > 5) throw new Exception("Image has size than more 5MB!");
                Image image = new Image();
                if (!image.Download(url)) return BadRequest(new ErrorMessageModel { Error = "Image already exists!" });              
                image.Cut(100);
                image.Cut(300);
                await context.images.AddAsync(image.imageModel);
                await context.SaveChangesAsync();
                string host = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host + "/";
                return Ok(new LinkModel { Url = host + image.imageModel.Path + image.imageModel.Name });
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
            var image = await context.images.FindAsync(id);
            if (image == null) return NotFound(new ErrorMessageModel { Error = "Record doesnot found!" });
            if (image.CutSizes != null)
            {
                string[] cut_sizes = image.CutSizes.Split('x');
                foreach (string cut_size in cut_sizes)
                    System.IO.File.Delete(image.Path + cut_size + "_" + image.Name);
            }
            System.IO.File.Delete(image.Path + image.Name);
            context.images.Remove(image);
            await context.SaveChangesAsync();
            return Ok(new MessageModel { message = "Successfully deleting!" });
        }

        private double GetPictureSize(string Url)
        {
            HttpClient webRequest = new HttpClient();
            using (var webResponse = webRequest.GetAsync(Url))
            {
                string[] fileSizeBytes = (string[])webResponse.Result.Content.Headers.GetValues("Content-Length");
                return Math.Round(Convert.ToDouble(fileSizeBytes[0]) / 1024.0 / 1024.0, 2);
            }
        }
       
    }
}
