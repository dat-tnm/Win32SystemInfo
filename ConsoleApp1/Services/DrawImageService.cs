using System;
using System.IO;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace ConsoleApp1.Models
{
    public class DrawImageService
    {

        public void DrawText(string imagePath, string newImagePath, string text, PointF pointF)
        {
            // Load an image
            using (var image = Image.Load<Rgba32>(imagePath))
            {
                // Create a font
                string programDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
                string fontPath = Path.Combine(programDirectory, "Roboto-Black.ttf");

                var fontCollection = new FontCollection();
                fontCollection.Add(fontPath);
                var font = fontCollection.Get("Roboto Black").CreateFont(22);

                var textColor = Color.White;

                // Add the text to the image
                image.Mutate(ctx => ctx.DrawText(text, font, textColor, pointF));

                image.Save(newImagePath);
                //Console.WriteLine($"Text added to {imagePath}. Modified image saved as {newImagePath}.");
            }
        }
    }
}
