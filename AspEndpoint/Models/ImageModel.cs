
using System.Drawing;
using System.Net;

namespace AspEndpoint.Models
{
    public class ImageModel
    {
        public string? Path { get; set; }
        public string? Name { get; set; }
        public void Download(string url)
        {
            WebClient client = new WebClient();
            string[] names = url.Split(new char[] { '/' });
            Name = names[^1];
            try
            {
                Path = Directory.GetCurrentDirectory() + "\\Images\\";
                client.DownloadFile(url, Path + Name);
            }
            catch (WebException)
            {
                Path = Directory.GetCurrentDirectory() + "\\";
                client.DownloadFile(url, Path + Name);

            }
        }

        public void Cut(int new_size)
        {
            if (Path != null && Name != null)
            {
                using (Bitmap bitmap = new Bitmap(Path + Name))
                {
                    Size size = GetNewPictureSize(bitmap.Size, new_size);
                    string new_path = String.Empty;
                    using (Bitmap newBitmap = new Bitmap(bitmap, size))
                    {
                        try
                        {
                            new_path = Path + "new_" + new_size + "_" + Name;
                            newBitmap.Save(new_path);
                        }
                        catch (Exception)
                        {
                            new_path = Path + "\\new_" + new_size + "_" + Name;
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
