using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;



namespace Project
{
    public class Image
    {
        public string Path { get; private set; }
        public string Name { get; private set; }

        public bool Download(string url)
        {
            WebClient client = new WebClient();
            string[] names = url.Split(new char[] { '/' });
            Name = names[names.Length - 1];
            try
            {
                Path = Directory.GetCurrentDirectory() + "\\Images\\";
                if (!File.Exists(Path))
                {
                    client.DownloadFile(url, Path + Name);
                    return true;
                }
                else return false;
            }
            catch (WebException)
            {
                Path = Directory.GetCurrentDirectory() + "\\";
                if (!File.Exists(Path))
                {
                    client.DownloadFile(url, Path + Name);
                    return true;
                }
                else return false;
            }
        }

        public Size GetNewPictureSize(Size old_size, int new_size)
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

        public void Cut(int new_size)
        {
            using (Bitmap bitmap = new Bitmap(Path+Name))
            {
                Size size = GetNewPictureSize(bitmap.Size, new_size);
                string new_path = String.Empty;
                using (Bitmap newBitmap = new Bitmap(bitmap, size))
                {
                    try
                    {
                        new_path = Directory.GetCurrentDirectory() + "\\Images\\new_" + new_size + "_" + Name;
                        newBitmap.Save(new_path);
                    }
                    catch (Exception)
                    {
                        new_path = Directory.GetCurrentDirectory() + "\\new_" + new_size + "_" + Name;
                        newBitmap.Save(new_path);
                    }
                    
                }
            }
        }
    }
}
