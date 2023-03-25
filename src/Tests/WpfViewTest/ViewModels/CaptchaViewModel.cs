using System.Windows.Input;
using CaptchaModel;
using ViewModelCommandBases;
using ViewModelCommandBases.Commands;
using static DataModels.Helpers.HashHandlers;

namespace WpfViewTest.ViewModels;

public class CaptchaViewModel : ViewModelBase
{
    private (string HashCaptchaCode, byte[] Image) _captcha 
        = Captcha.GenerateImageAsByteArray();

    private string _title = "Captcha Test - не пройден...";

    public CaptchaViewModel()
    {
        RefreshCommand = new CommonCommand(Refresh);
    }

    private void Refresh()
    {
        _captcha = Captcha.GenerateImageAsByteArray();
        Title = "Captcha Test - не пройден...";
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
            if (VerifyHashedString(_captcha.HashCaptchaCode, value, true))
                Title = "Ура! Test - пройден!";
        }
    }

    public ICommand RefreshCommand { get; }
}