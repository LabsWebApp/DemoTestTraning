using System;
using System.Windows.Input;
using CoreModel;
using ViewModelCommandsBases;
using ViewModelCommandsBases.Commands;

namespace WpfViewTest.ViewModels;

public class CaptchaViewModel : ViewModelBase
{
    private (string HashCaptchaCode, byte[] Image) _captcha 
        = Captcha.GenerateImageAsByteArray();

    private string _title = "Captcha Test - не пройден...";

    public CaptchaViewModel()
    {
        RefreshCommand = new SimpleCommand(Refresh);
    }

    private void Refresh()
    {
        Title = "Captcha Test - не пройден...";
        _captcha = Captcha.GenerateImageAsByteArray();
        OnPropertyChanged(nameof(CaptchaImage));
    }

    public string Title
    {
        get => _title;
        set => Set(ref _title, value);
    }

    public byte[] CaptchaImage => _captcha.Image;

    public string Text
    {
        set
        {
            if (Captcha.VerifyHashedString(_captcha.HashCaptchaCode, value, true))
                Title = "Ура! Test - пройден!";
        }
    }

    public ICommand RefreshCommand { get; }
}