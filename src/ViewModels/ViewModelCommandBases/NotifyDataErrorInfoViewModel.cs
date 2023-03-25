using System.Collections;
using System.ComponentModel;

namespace ViewModelCommandBases;

public abstract class NotifyDataErrorInfoViewModel : ViewModelBase, INotifyDataErrorInfo
{
    private readonly Dictionary<string, IList<string>?> _errorsByPropertyName = new();

    public IEnumerable GetErrors(string? propertyName) =>
        _errorsByPropertyName.TryGetValue(propertyName ?? string.Empty, out var result) 
            ? result ?? Enumerable.Empty<string>() : Enumerable.Empty<string>();

    public bool HasErrors => _errorsByPropertyName.Any();

    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

    protected void OnErrorsChanged(string propertyName) =>
        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));

    protected void AddError(string propertyName, string error)
    {
        if (!_errorsByPropertyName.ContainsKey(propertyName))
            _errorsByPropertyName[propertyName] = new List<string>();

        if (!_errorsByPropertyName[propertyName]!.Contains(error))
        {
            _errorsByPropertyName[propertyName]!.Add(error);
            OnErrorsChanged(propertyName);
        }
    }

    protected void ClearErrors(string propertyName)
    {
        if (_errorsByPropertyName.Remove(propertyName)) OnErrorsChanged(propertyName);
    }

    protected void SyncCollection(string propertyName, ICollection<string> collection)
    {
        var errors = GetErrors(propertyName)?.Cast<string>().ToArray();
        if (errors is null || !errors.Any())
        {
            _errorsByPropertyName.Remove(propertyName);
            collection.Clear();
            return;
        }
        foreach (var item in collection.Except(errors)) collection.Remove(item);
        foreach (var item in errors.Except(collection)) collection.Add(item);
    }
}