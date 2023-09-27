namespace TMPS.Common;

public class OptionsHandler<T> where T : notnull
{
    private Dictionary<T, Action> _optionHandlers = new();
    private Action _defaultFallback = HandleDefaultFallback;
    private static void HandleDefaultFallback()
    {
        Console.WriteLine("Invalid option.");
    }
    public OptionsHandler<T> AddOption(T option, Action action)
    {
        _optionHandlers.Add(option, action);
        return this;
    }

    public OptionsHandler<T> SetDefaultFallback(Action action)
    {
        _defaultFallback = action;
        return this;
    }

    public void HandleOption(T option)
    {
        if (_optionHandlers.ContainsKey(option))
        {
            _optionHandlers[option]();
        }
        else
        {
            _defaultFallback();
        }
    }
}