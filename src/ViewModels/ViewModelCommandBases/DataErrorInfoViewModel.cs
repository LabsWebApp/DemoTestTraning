using System.ComponentModel;

namespace ViewModelCommandBases;

public abstract class DataErrorInfoViewModel : ViewModelBase, IDataErrorInfo
{
    public string Error { get; protected set; } = string.Empty;

    public abstract string this[string propertyName] { get; }
}