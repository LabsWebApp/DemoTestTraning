using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ViewModels;

public class CaptchaCommonViewModel : ErrorInfoViewModel
{
    private (string Code, byte[] Image) _model;
    private const string PropertyName = nameof(CaptchaCode);

    public CaptchaCommonViewModel()
    {
        RefreshCaptcha = new CommonCommand(RefreshModel, ()=> !CaptchaOk);
        _model = CaptchaModel.Captcha.GenerateImageAsByteArray();
        _captchaImage = _model.Image;

        CodeLength = DefaultCodeLength;
        ErrorsChanged += (_, args) =>
        {
            if (args.PropertyName == PropertyName)
                AdaptCollection(PropertyName, CaptchaErrors);
        };
    }

    public ICommand RefreshCaptcha { get; }

    public ObservableCollection<string> CaptchaErrors { get; } = new();

    public int CodeLength { get; }

    private byte[] _captchaImage;

    public byte[] CaptchaImage
    {
        get => _captchaImage;
        set => Set(ref _captchaImage, value);
    }

    private string _captchaCode = string.Empty;
    public string CaptchaCode
    {
        get => _captchaCode;
        set
        {
            if (!Set(ref _captchaCode, value)) return;

            ClearErrors(PropertyName);
            if (string.IsNullOrEmpty(value) || value.Length < CodeLength)
            {
                if (value.Length > 0)
                    AddError(PropertyName, ErrorsDictionary[Errors.InputTextIsTooSmall]);
            }
            else 
            {
                if (!VerifyHashedString(value, _model.Code, true))
                    AddError(PropertyName, ErrorsDictionary[Errors.CaptchaIsNotValid]);
            }
            OnPropertyChanged(nameof(CaptchaOk));
            OnErrorsChanged(PropertyName);
        }
    }

    public bool CaptchaOk => _captchaCode.Length == CodeLength && !CaptchaErrors.Any();

    private void RefreshModel()
    {
        CaptchaCode = string.Empty;
        ClearErrors(PropertyName);
        _model = CaptchaModel.Captcha.GenerateImageAsByteArray();
        CaptchaImage = _model.Image;
    }
}