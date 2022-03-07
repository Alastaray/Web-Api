namespace AspEndpoint.Services
{
    public class ImageDownloadService
    {
        private readonly ImageContext _imageContext;
        public ImageDownloadService(ImageContext context)
        {
            _imageContext = context;
        }
        public async Task<string> ProcessImageAsync(string url)
        {
            if (GetPictureSize(url) > 5) throw new Exception("Image has size than more 5MB!");
            Picture picture = new Picture();            
            if(!await TryDownloadImageAsync(picture, url)) 
                throw new Exception("Image already exists!");
            await picture.CutAsync(100);
            await picture.CutAsync(300);
            await _imageContext.images.AddAsync(picture.imageModel);
            await _imageContext.SaveChangesAsync();
            return picture.imageModel.Path + picture.imageModel.Name;
        }
        public async Task<bool> TryDownloadImageAsync(Picture picture, string url)
        {
            bool result = false;
            try
            {
                result = await picture.DownloadAsync(url, Directory.GetCurrentDirectory() + "\\Images\\");
            }
            catch (Exception)
            {
                result = await picture.DownloadAsync(url, Directory.GetCurrentDirectory() + "\\");
            }
            return result;
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
