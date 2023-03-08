namespace ViewModels;

public class TestViewModel : CaptchaCommonViewModel
{
    public bool TestOk => CaptchaOk;

    public TestViewModel() : base() => ErrorsChanged += (_, _) => OnPropertyChanged(nameof(TestOk));
}