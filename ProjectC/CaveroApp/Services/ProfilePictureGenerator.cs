using SkiaSharp;
using System.IO;

public class ProfilePictureGenerator
{
    private static readonly SKColor[] Colors = new[]
    {
        SKColors.Aquamarine, SKColors.Coral, SKColors.Tomato, SKColors.Goldenrod, SKColors.Plum
    };

    /// <summary>
    /// Generates a profile picture for a user.
    /// </summary>
    /// <param name="firstName">The first name of the user. The first letter of this name will be used in the profile picture.</param>
    /// <param name="lastName">The last name of the user. The first letter of this name will be used in the profile picture.</param>
    /// <returns>
    /// A stream containing the image data of the generated profile picture.
    /// </returns>
    /// <remarks>
    /// This method generates a profile picture by creating a bitmap and drawing a circle with a random color as the background,
    /// and the initials of the user as the text. The text color is a darker version of the background color.
    /// The bitmap is then encoded as a PNG and returned as a stream.
    /// </remarks>
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

    /// <summary>
    /// Generates a profile picture for a user and saves it to a file.
    /// </summary>
    /// <param name="firstName">The first name of the user. The first letter of this name will be used in the profile picture.</param>
    /// <param name="lastName">The last name of the user. The first letter of this name will be used in the profile picture.</param>
    /// <param name="ID">The ID of the user. This will be used to name the file.</param>
    /// <remarks>
    /// This method generates a profile picture by calling the Generate method of the ProfilePictureGenerator class,
    /// which returns a stream containing the image data. This stream is then copied to a new file in the wwwroot/img/profilepictures directory.
    /// The file is named "avatar" followed by the user's ID and the .png extension.
    /// </remarks>
    public static void MakePF(string firstName, string lastName, string ID)
    {
        using (var stream = new ProfilePictureGenerator().Generate(firstName, lastName))
        using (var fileStream = File.Create($"wwwroot/img/profilepictures/avatar{ID}.png"))
        {
            stream.CopyTo(fileStream);
        }
    }
}