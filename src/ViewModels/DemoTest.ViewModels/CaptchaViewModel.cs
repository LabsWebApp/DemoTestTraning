using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ViewModelCommandsBases;
using ViewModelCommandsBases.Commands;
using static DataModels.Helpers.HashHandlers;

namespace ViewModels
{
    public class CaptchaViewModel : ViewModelBase
    {
        private (string Code, byte[] Img) _model;

        public CaptchaViewModel()
        {
            Refresh = new CommonCommand(RefreshModel);
            _model = CaptchaModel.Captcha.GenerateImageAsByteArray();
            _image = _model.Img;

            _codeLength = DefaultCodeLength;
        }

        private void RefreshModel()
        {
            _model = CaptchaModel.Captcha.GenerateImageAsByteArray();
            Image = _model.Img;
            Code = string.Empty;
            Focus?.Invoke();
        }

        public ICommand Refresh { get; }

        public Action? Ok { private get; set; }
        public Action? Focus { private get; set; }

        private byte[] _image;

        public byte[] Image
        {
            get => _image;
            set => Set(ref _image, value);
        }

        private string? _code;
        public string? Code
        {
            get => _code;
            set
            {
                if (!Set(ref _code, value)) return;
                if (string.IsNullOrEmpty(value) || value.Length < DefaultCodeLength)
                {
                    Error = false;
                    return;
                }
                if (VerifyHashedString(value, _model.Code, true))
                    Ok?.Invoke();
                else Error = true;
            }
        }

        private bool _error = false;
        public bool Error
        {
            get => _error;
            set => Set(ref _error, value);
        }

        private int _codeLength;
        public int CodeLength => _codeLength;
    }
}
