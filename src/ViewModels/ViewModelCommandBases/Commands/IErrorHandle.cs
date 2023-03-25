namespace ViewModelCommandBases.Commands;

public interface IErrorHandle
{
    void ErrorHandle(Exception exception);
}