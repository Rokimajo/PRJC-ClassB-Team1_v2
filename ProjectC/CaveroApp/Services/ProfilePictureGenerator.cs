using SkiaSharp;
using System.IO;

public class ProfilePictureGenerator
{
    private static readonly SKColor[] Colors = new[]
    {
        SKColors.Aquamarine, SKColors.Coral, SKColors.Tomato, SKColors.Goldenrod, SKColors.Plum
    };

    public Stream Generate(string firstName, string lastName)
    {
        int width = 100, height = 100;
        var bitmap = new SKBitmap(width, height);
        var canvas = new SKCanvas(bitmap);
        var rand = new Random();
        var typeface = SKTypeface.FromFile("wwwroot/img/profilepictures/fonts/Montserrat-Medium.ttf");
        var color = Colors[rand.Next(0, Colors.Length - 1)];
            
        // Set up the paint for the background
        var backgroundPaint = new SKPaint
        {
            Color = color,
            IsAntialias = true
        };

        // Set up the paint for the text
        var textPaint = new SKPaint
        {
            // make the text color a darker version of the background color.
            Color = new SKColor((byte)(color.Red * 0.3), (byte)(color.Green * 0.3), (byte)(color.Blue * 0.3)),
            IsAntialias = true,
            Style = SKPaintStyle.Fill,
            TextAlign = SKTextAlign.Center,
            TextSize = 40,
            Typeface = typeface,
        };

        // Draw the background
        canvas.DrawCircle(width / 2, height / 2, width / 2, backgroundPaint);

        // Draw the text
        canvas.DrawText($"{firstName[0]}{lastName[0]}", width / 2, (height + textPaint.TextSize) / 2 - 5, textPaint);

        // Encode the bitmap as a PNG and return a stream
        var image = SKImage.FromBitmap(bitmap);
        var data = image.Encode(SKEncodedImageFormat.Png, 100);
        return data.AsStream();
    }

    public static void MakePF(string firstName, string lastName, string ID)
    {
        using (var stream = new ProfilePictureGenerator().Generate(firstName, lastName))
        using (var fileStream = File.Create($"wwwroot/img/profilepictures/avatar{ID}.png"))
        {
            stream.CopyTo(fileStream);
        }
    }
}