using AspEndpoint.Models;

namespace AspEndpoint.Services
{
    public class ImageRemoveServise
    {
        private readonly ImageContext context;
        public ImageRemoveServise(ImageContext _context)
        {
            context = _context;
        }
        public async Task<string> RemoveImage(int id)
        {
            var image = await context.images.FindAsync(id);
            if (image == null) throw new Exception("Record doesnot found!");
            DeleteFiles(image);
            context.images.Remove(image);
            await context.SaveChangesAsync();
            return "Successfully deleting!";
        }
        public void DeleteFiles(ImageModel image)
        {
            if (image.CutSizes != null)
            {
                string[] cut_sizes = image.CutSizes.Split('x');
                foreach (string cut_size in cut_sizes)
                    System.IO.File.Delete(image.Path + cut_size + "_" + image.Name);
            }
            File.Delete(image.Path + image.Name);
        }
    }
}
