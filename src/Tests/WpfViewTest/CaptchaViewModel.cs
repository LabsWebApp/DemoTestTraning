using System;
using CoreModel;
using ViewModelCommandsBases;

namespace WpfViewTest;

public class CaptchaViewModel : ViewModelBase
{
    private readonly (string, byte[]) _captcha = Captcha.GenerateImageAsByteArray();

    private string _title = "Captcha Test - не пройден";
    public string Title
    {
        get => _title;
        set => Set(ref _title, value);
    }

    public byte[] CaptchaImage => _captcha.Item2;

    public string Text
    {
        set
        {
            if (Captcha.VerifyHashedString(_captcha.Item1, value))
                Title = "Captcha Test - пройден!";
        }
    }
}