using System;
using System.Drawing;
using System.IO;
using System.Net;



namespace Project
{
    public class Picture
    {
        public string Path { get; private set; }
        public string NewPath { get; private set; }
        public string Name { get; private set; }

        public void Download(string url)
        {
            WebClient client = new WebClient();
            string[] names = url.Split(new char[] { '/' });
            Name = names[names.Length - 1];
            try
            {
                Path = Directory.GetCurrentDirectory() + "\\Images\\" + Name;
                client.DownloadFile(url, Path);
            }
            catch (WebException)
            {
                Path = Directory.GetCurrentDirectory() + "\\" + Name;
                client.DownloadFile(url, Path);
            }
        }

        public Size GetNewPictureSize(Size size)
        {
            int width = size.Width,
               height = size.Height,
               width_modifier = width / 100,
               height_modifier = height / 100,
               modifier = width_modifier.Equals(height_modifier) ? width_modifier : height_modifier;
            return new Size(width / modifier, height / modifier);
        }

        public void Cut()
        {
            using (Bitmap bitmap = new Bitmap(Path))
            {
                Size size = GetNewPictureSize(bitmap.Size);
                using (Bitmap newBitmap = new Bitmap(bitmap, size))
                {
                    try
                    {
                        NewPath = Directory.GetCurrentDirectory() + "\\Images\\new_" + Name;
                        newBitmap.Save(NewPath);
                    }
                    catch (Exception)
                    {
                        NewPath = Directory.GetCurrentDirectory() + "\\new_" + Name;
                        newBitmap.Save(NewPath);
                    }
                    
                }
            }
        }
    }
}
