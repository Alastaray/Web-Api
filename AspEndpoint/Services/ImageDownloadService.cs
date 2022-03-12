namespace AspEndpoint.Services
{
    public class ImageDownloadService : ImageService
    {
        public ImageDownloadService(ImageContext context, IConfiguration configuration) : base(context, configuration) { }
        public async Task<string> ProcessImageAsync(string url)
        {
            int maxFileSize = int.Parse(_config["MaxFileSize"] ?? "5");
            if (GetPictureSize(url) > maxFileSize) 
                throw new Exception("Image has size than more " + maxFileSize + "MB!");
            Picture picture = new Picture();            
            if(!await picture.DownloadAsync(url, Directory.GetCurrentDirectory() + "\\Images")) 
                throw new Exception("Image already exists!");
            await picture.CutAsync(100);
            await picture.CutAsync(300);
            await _imageContext.images.AddAsync(picture.imageModel);
            await _imageContext.SaveChangesAsync();
            return picture.imageModel.Path + picture.imageModel.Name;
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
