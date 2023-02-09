using System.Security.Cryptography;
using System.Text;
using CaptchaGen.SkiaSharp;
using SkiaSharp;

namespace CoreModel;

public static class Captcha
{
    public const int
        MinImageWidth = 120,
        MinImageHeight = 48,
        CodeLength = 8,
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
                   Math.Min(imageWidth - MinImageWidth, imageHeight - MinImageHeight) / CodeLength;

        Random r = new();
        var code = new StringBuilder(CodeLength);
        for (var i = 0; i < CodeLength; i++)
            switch (r.Next(3))
            {
                case 0:
                    code.Append((char)r.Next('A', 'Z' + 1));
                    break;
                case 1:
                    code.Append((char)r.Next('a', 'z' + 1));
                    break;
                default:
                    code.Append((char)r.Next('0', '9' + 1));
                    break;
            }

        return (
            GetHashString(code.ToString()),
            new CaptchaGenerator(
                paintColorHex, backgroundColorHex, noisePointColorHex,
                imageWidth, imageHeight,
                fontSize: size)
                    .GenerateImageAsByteArray(code.ToString(), SKEncodedImageFormat.Png));
    }

    public static string GetHashString(string code, bool useLowercase = true)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Пустая строка не пригодна для хэширования", nameof(code));

        if (useLowercase) code = code.ToLower();

        return string.Concat(SHA256.HashData(
            Encoding.UTF8.GetBytes(code)).Select(x => x.ToString("X2")));
    }

    public static bool VerifyHashedString(string hashedCode, string? code, bool useLowercase = true)
    {
        if (string.IsNullOrWhiteSpace(code)) return false;
        if (useLowercase) code = code.ToLower();
        return hashedCode == GetHashString(code);
    }
}