using AspEndpoint.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace AspEndpoint.Controllers
{
    [ApiController]  

    public class ImageController : ControllerBase
    {
        
        [HttpPost]
        [Route("api/upload-by-url")]
        public async Task<IActionResult> UploadByUrl(string url)
        {
            try
            {
                if (GetPictureSize(url) > 5) throw new Exception("Image has size than more 5MB!");
                ImageModel imagemodel = new ImageModel();
                imagemodel.Download(url);
                imagemodel.Cut(100);
                imagemodel.Cut(300);
                string myAddress = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host + "/";
                return Ok(new LinkModel { Url = myAddress + imagemodel.Path + imagemodel.Name });
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
           
            string myAddress = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host + "/";
            return Ok(new ErrorMessageModel { Error = "da"+id});
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
