using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Natmc.Resources.Textures
{
    public static class ImageLoader
    {
        public static ImageData? Load(string resourcePath)
        {
            foreach (var pack in ResourceManager.LoadedPacks)
            {
                var reader = pack.PackReader;
                if (!reader.FileExists(resourcePath))
                    continue;
                return Load(reader.OpenFile(resourcePath));
            }
            return null;
        }

        private static ImageData Load(Stream stream)
        {
            using var image = Image.Load<Rgba32>(stream);

            var data = new byte[image.Width * image.Height * 4];
            for (var y = 0; y < image.Height; y += 1)
            {
                var span = image.GetPixelRowSpan(image.Height - y - 1);
                for (var x = 0; x < span.Length; x += 1)
                {
                    data[y * image.Width * 4 + x * 4 + 0] = span[x].R;
                    data[y * image.Width * 4 + x * 4 + 1] = span[x].G;
                    data[y * image.Width * 4 + x * 4 + 2] = span[x].B;
                    data[y * image.Width * 4 + x * 4 + 3] = span[x].A;
                }
            }

            return new ImageData
            {
                Data = data,
                Width = image.Width,
                Height = image.Height,
            };
        }
    }
}
