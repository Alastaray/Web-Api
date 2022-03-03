using AspEndpoint.Models;

using System.Drawing;
using System.Net;

namespace AspEndpoint
{
    public class Image
    {
        public ImageModel imageModel { get; set; }

        public Image()
        {
            imageModel = new ImageModel();
        }

        public bool Download(string url)
        {
            WebClient client = new WebClient();
            string[] names = url.Split(new char[] { '/' });
            imageModel.Name = names[^1];
            try
            {
                imageModel.Path = Directory.GetCurrentDirectory() + "\\Images\\";
                if (!File.Exists(imageModel.Path + imageModel.Name))
                {
                    client.DownloadFile(url, imageModel.Path + imageModel.Name);
                    return true;
                }
                else return false;
            }
            catch (WebException)
            {
                imageModel.Path = Directory.GetCurrentDirectory() + "\\";
                if (!File.Exists(imageModel.Path + imageModel.Name))
                {
                    client.DownloadFile(url, imageModel.Path + imageModel.Name);
                    return true;
                }
                else return false;
            }
        }

        public void Cut(int new_size)
        {
            if (imageModel.Path != null && imageModel.Name != null)
            {
                imageModel.CutSizes += new_size + "x";
                using (Bitmap bitmap = new Bitmap(imageModel.Path + imageModel.Name))
                {
                    Size size = GetNewPictureSize(bitmap.Size, new_size);
                    string new_path = String.Empty;
                    using (Bitmap newBitmap = new Bitmap(bitmap, size))
                    {
                        try
                        {
                            new_path = imageModel.Path + new_size + "_" + imageModel.Name;
                            newBitmap.Save(new_path);
                        }
                        catch (Exception)
                        {
                            new_path = imageModel.Path + "\\" + new_size + "_" + imageModel.Name;
                            newBitmap.Save(new_path);
                        }

                    }
                }
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
