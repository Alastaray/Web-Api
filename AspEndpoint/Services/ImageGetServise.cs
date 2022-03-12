﻿using AspEndpoint.Models;

namespace AspEndpoint.Services
{
    public class ImageGetServise : ImageService
    {
        public ImageGetServise(ImageContext context, IConfiguration configuration) : base(context, configuration) { }
        public async Task<ImageModel> GetImageAsync(int id)
        {
            var image = await _imageContext.images.FindAsync(id);
            if (image == null) throw new Exception("Record doesnot found!");
            return image;
        }
    }
}
