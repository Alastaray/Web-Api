using AspEndpoint.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace AspEndpoint
{
    public class Picture
    {
        public FileModel fileModel { get; set; }

        public Picture(FileModel _fileModel)
        {
            fileModel = _fileModel;
        }

        public void Cut(byte[] file, int newSize)
        {
            if (fileModel.Path != null && fileModel.Name != null)
            {
                var image = Image.Load(file);
                image.Mutate(x => x.Resize(GetNewPictureSize(image.Size(), newSize)));
                fileModel.CutSizes += newSize + "x";
                string newPath = String.Empty;
                newPath = fileModel.Path + newSize + "_" + fileModel.Name;
                image.Save(newPath);
            }
            else throw new Exception("fileModel is empty!");
        }


        private Size GetNewPictureSize(Size old_size, int newSize)
        {
            if (newSize != 0)
            {
                int width = old_size.Width,
                height = old_size.Height,
                width_modifier = width / newSize,
                height_modifier = height / newSize,
                modifier = width_modifier.Equals(height_modifier) ? width_modifier : height_modifier;
                return new Size(width / modifier, height / modifier);
            }
            else
                return old_size;
        }
    }
}
