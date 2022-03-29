using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace AspEndpoint.Helpers
{
    public static class Picture
    {
        static public byte[]? Cut(byte[] file, int size)
        {
            using var memoryStream = new MemoryStream();
            var image = Image.Load(file);
            Size oldSize = image.Size();
            Size newSize = GetNewPictureSize(oldSize, size);
            if(!oldSize.Equals(newSize))
            {
                image.Mutate(x => x.Resize(newSize));
                image.SaveAsJpeg(memoryStream);
                return memoryStream.ToArray();
            }
            return null;
        }

        static private Size GetNewPictureSize(Size oldSize, int newSize)
        {
            if (newSize != 0)
            {
                int width = oldSize.Width,
                    height = oldSize.Height;
                if(width > height)
                {
                    double modifier = width * 1.0 / height;
                    return new Size((int)(modifier * newSize), newSize);
                }
                else
                {
                    double modifier = height * 1.0 / width;
                    return new Size(newSize, (int)(modifier * newSize));
                }                  
            }
            else
                return oldSize;
        }
    }
}
