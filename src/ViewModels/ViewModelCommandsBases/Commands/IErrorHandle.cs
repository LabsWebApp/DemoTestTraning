namespace ViewModelCommandsBases.Commands;

public interface IErrorHandle
{
    void ErrorHandle(Exception exception);
}