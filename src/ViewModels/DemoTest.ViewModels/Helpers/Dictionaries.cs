namespace ViewModels.Helpers;

public static class Dictionaries
{
    public enum Errors
    {
        CaptchaIsNotValid, InputTextIsTooSmall
    }

    public static readonly Dictionary<Errors, string> ErrorsDictionary = new()
    {
        {
            Errors.CaptchaIsNotValid, "Введённый текст не соответствует изображению!"
        },
        {
            Errors.InputTextIsTooSmall, "Ввод не закончен - недостаточно символов."
        },
    };
}