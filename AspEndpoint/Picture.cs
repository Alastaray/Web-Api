using AspEndpoint.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System.Net;

namespace AspEndpoint
{
    public class Picture
    {
        public ImageModel imageModel { get; set; }

        public Picture()
        {
            imageModel = new ImageModel();
        }

        public async Task<bool> DownloadAsync(string url)
        {
            HttpClient httpClient = new HttpClient();
            imageModel.Name = url.Split(new char[] { '/' })[^1];
            try
            { 
                var file = await httpClient.GetByteArrayAsync(url);
                try
                {
                    imageModel.Path = Directory.GetCurrentDirectory() + "\\Images\\";
                    if (!File.Exists(imageModel.Path + imageModel.Name))
                    {
                        File.WriteAllBytes(imageModel.Path + imageModel.Name, file);
                        return true;
                    }
                    else return false;
                }
                catch (WebException)
                {
                    imageModel.Path = Directory.GetCurrentDirectory() + "\\";
                    if (!File.Exists(imageModel.Path + imageModel.Name))
                    {
                        File.WriteAllBytes(imageModel.Path + imageModel.Name, file);
                        return true;
                    }
                    else return false;
                }
            }
            catch (Exception)
            {
                throw new Exception("Url is incorrect!");
            }           
        }
        public async Task CutAsync(int new_size)
        {
            if (imageModel.Path != null && imageModel.Name != null)
            {
                var image = await Image.LoadAsync(imageModel.Path + imageModel.Name);
                image.Mutate(x => x.Resize(GetNewPictureSize(image.Size(), new_size)));
                string new_path = String.Empty;
                new_path = imageModel.Path + new_size + "_" + imageModel.Name;
                await image.SaveAsync(new_path);
            }
            else throw new Exception("ImageModel is empty!");
        }
        public void Cut(int new_size)
        {
            if (imageModel.Path != null && imageModel.Name != null)
            {
                var image = Image.Load(imageModel.Path + imageModel.Name);
                image.Mutate(x => x.Resize(GetNewPictureSize(image.Size(), new_size)));
                string new_path = String.Empty;
                new_path = imageModel.Path + new_size + "_" + imageModel.Name;
                image.Save(new_path);
            }
            else throw new Exception("ImageModel is empty!");
        }


        private Size GetNewPictureSize(Size old_size, int new_size)
        {
            if (new_size != 0)
            {
                int width = old_size.Width,
                height = old_size.Height,
                width_modifier = width / new_size,
                height_modifier = height / new_size,
                modifier = width_modifier.Equals(height_modifier) ? width_modifier : height_modifier;
                return new Size(width / modifier, height / modifier);
            }
            else
                return old_size;
        }
    }
}
