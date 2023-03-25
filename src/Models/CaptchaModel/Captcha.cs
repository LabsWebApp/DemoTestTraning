using CaptchaGen.SkiaSharp;
using SkiaSharp;
using static DataModels.Helpers.HashHandlers;

namespace CaptchaModel;

public static class Captcha
{
    public const int
        MinImageWidth = 120,
        MinImageHeight = 48,
        CodeFontSize = 20;

    private const string
        DefaultPaintColorHex = "#808080",
        DefaultBackgroundColorHex = "#F5DEB3",
        DefaultNoisePointColorHex = "#D3D3D3";

    public static (string captchaHashCode, byte[] image) GenerateImageAsByteArray(
        string paintColorHex = DefaultPaintColorHex,
        string backgroundColorHex = DefaultBackgroundColorHex,
        string noisePointColorHex = DefaultNoisePointColorHex,
        int imageWidth = MinImageWidth, int imageHeight = MinImageHeight)
    {
        if (imageHeight < MinImageHeight || imageWidth < MinImageWidth)
            throw new ArgumentException("Размеры поля для отображения капчи слишком малы");

        var size = CodeFontSize +
                   Math.Min(imageWidth - MinImageWidth, imageHeight - MinImageHeight) / DefaultCodeLength;

        var code = SaltGenerator();

        return (
            GetHashString(code),
            new CaptchaGenerator(
                paintColorHex, backgroundColorHex, noisePointColorHex,
                imageWidth, imageHeight,
                fontSize: size)
                    .GenerateImageAsByteArray(code.ToString(), SKEncodedImageFormat.Png));
    }

}