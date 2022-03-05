using AspEndpoint.Models;

namespace AspEndpoint.Services
{
    public class ImageDownloadService
    {
        private readonly ImageContext context;
        public ImageDownloadService(ImageContext _context)
        {
            context = _context;
        }
        public async Task<string> DownloadImageAsync(string url)
        {
            if (GetPictureSize(url) > 5) throw new Exception("Image has size than more 5MB!");
            Picture image = new Picture();
            bool result = false;
            try
            {
                result = await image.DownloadAsync(url, Directory.GetCurrentDirectory() + "\\Image\\");            
            }
            catch (Exception)
            {
                result = await image.DownloadAsync(url);
            }
            if(!result) throw new Exception("Image already exists!");
            await image.CutAsync(100);
            await image.CutAsync(300);
            await context.images.AddAsync(image.imageModel);
            await context.SaveChangesAsync();
            return image.imageModel.Path + image.imageModel.Name;
        }


        public double GetPictureSize(string Url)
        {
            HttpClient webRequest = new HttpClient();
            var webResponse = webRequest.GetAsync(Url);
            string[] fileSizeBytes = (string[])webResponse.Result.Content.Headers.GetValues("Content-Length");
            return Math.Round(Convert.ToDouble(fileSizeBytes[0]) / 1024.0 / 1024.0, 2);

        }
    }
}
