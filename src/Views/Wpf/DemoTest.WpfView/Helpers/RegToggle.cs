using System;
using System.Linq;
using Microsoft.Win32;

namespace DemoTest.WpfView.Helpers;

public class RegToggle
{
    public enum Toggle
    {
        On = 1,
        Off = 0
    }

    public RegistryKey? SubKey;

    public string? ErrorMessage;

    private readonly (string, RegistryValueKind)[]? _values;

    public RegToggle(RegistryKey root, string subKey, params string[] values)
    {
        try
        {
            SubKey = root?.OpenSubKey(subKey, true);
            if (SubKey is null)
            {
                ErrorMessage = $"{root}//{subKey} не существует.";
                return;
            }

            _values = new (string, RegistryValueKind)[values.Length];
            for (var i = 0; i < values.Length; i++)
            {
                var kind = SubKey!.GetValueKind(values[i]);
                if (kind is not RegistryValueKind.Unknown 
                    && (SubKey!.GetValue(values[i])?.ToString()?.EndsWith(((int)Toggle.On).ToString()) ?? false))
                    _values[i] = (values[i], kind);
            }

            if (_values?.All(v => v == default) ?? true)
                throw new Exception("Все переключатели в состоянии 'OFF'.");
        }
        catch (Exception e)
        {
            ErrorMessage = e.Message;
            _values = null;
        }
    }

    public RegToggle(string subKey, params string[] values) :
        this(Registry.CurrentUser, subKey, values)
    { }

    public void Switch(Toggle onOff)
    {
        if (_values == null) return;
        try
        {
            foreach (var (name, kind) in _values)
                if (!string.IsNullOrEmpty(name)) 
                    SubKey!.SetValue(name, (int)onOff, kind);
        }
        catch (Exception e)
        {
            ErrorMessage = e.Message;
        }
    }
}