using System.Security.Cryptography;
using System.Text;

namespace DataModels.Helpers;

public static class HashHandlers
{
#if DEBUG
    public const int DefaultCodeLength = 2;
#else
    public const int DefaultCodeLength = 8;
#endif

    public static string SaltGenerator(int length = DefaultCodeLength, int maxLength = 0)
    {
        Random r = new();
        if (length <= 0) length = DefaultCodeLength;
        if (maxLength > length) length = r.Next(length, maxLength + 1);

        var code = new StringBuilder(length);
        for (var i = 0; i < length; i++)
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

        return code.ToString();
    }

    public static string GetHashString(string code, bool useLowercase = true, string salt = "")
    {
        if (!string.IsNullOrWhiteSpace(salt)) code += salt;

        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Пустая строка не пригодна для хэширования", nameof(code));

        if (useLowercase) code = code.ToLower();

        return string.Concat(SHA256.HashData(
            Encoding.UTF8.GetBytes(code.Trim())).Select(x => x.ToString("X2")));
    }

    public static bool VerifyHashedString(string? code, string hashedCode, bool useLowercase, string salt = "")
    {
        if (string.IsNullOrWhiteSpace(code)) return false;
        code = code.Trim();
        if (!string.IsNullOrWhiteSpace(salt)) code += salt;
        if (useLowercase) code = code.ToLower();
        return hashedCode == GetHashString(code);
    }
}