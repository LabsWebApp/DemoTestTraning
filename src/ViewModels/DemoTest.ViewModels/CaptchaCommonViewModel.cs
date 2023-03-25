using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ViewModels;

public abstract class CaptchaCommonViewModel : NotifyDataErrorInfoViewModel
{
    private (string? Code, byte[]? Image) _model;
    private const string PropertyName = nameof(CaptchaCode);

    private protected CaptchaCommonViewModel()
    {
        RefreshCaptcha = new CommonCommand(RefreshModel, ()=> !CaptchaOk);
        _model = CaptchaModel.Captcha.GenerateImageAsByteArray();
        _captchaImage = _model.Image;

        CodeLength = DefaultCodeLength;
        ErrorsChanged += (_, args) =>
        {
            if (args.PropertyName == PropertyName)
            {
                SyncCollection(PropertyName, CaptchaErrors);
                OnPropertyChanged(nameof(CaptchaOk));
            }
        };
    }

    public ICommand RefreshCaptcha { get; }

    public ObservableCollection<string> CaptchaErrors { get; } = new();

    public int CodeLength { get; }

    private byte[]? _captchaImage;

    public byte[]? CaptchaImage
    {
        get => _captchaImage;
        set => Set(ref _captchaImage, value);
    }

    private string? _captchaCode;
    public string? CaptchaCode
    {
        get => _captchaCode;
        set
        {
            if (!Set(ref _captchaCode, value?.Trim())) return;

            ClearErrors(PropertyName);
            if (string.IsNullOrEmpty(_captchaCode) || _captchaCode.Length < CodeLength)
            {
                if (_captchaCode!.Length > 0)
                    AddError(PropertyName, ErrorsDictionary[Errors.InputTextIsTooSmall]);
            }
            else
            {
                if (_model.Code != null 
                    && !VerifyHashedString(_captchaCode, _model.Code, true))
                {
                    AddError(PropertyName, ErrorsDictionary[Errors.CaptchaIsNotValid]);
                }
                else
                {
                    _captchaImage = null;
                    _model.Image = null;
                    _model.Code = null;
                }
            }
        }
    }

    public bool CaptchaOk => _captchaCode?.Length > 0 && !CaptchaErrors.Any();

    private void RefreshModel()
    {
        CaptchaCode = string.Empty;
        ClearErrors(PropertyName);
        _model = CaptchaModel.Captcha.GenerateImageAsByteArray();
        CaptchaImage = _model.Image;
    }
}