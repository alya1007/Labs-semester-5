using ConsoleTools;

namespace TMPS.Common;

public class MenuHandler
{
    private readonly List<string> _options;
    private readonly List<Action> _actions;

    public MenuHandler(List<string> options, List<Action> actions)
    {
        _options = options;
        _actions = actions;
    }

    public void ShowMenu()
    {
        var menu = new ConsoleMenu();
        foreach (var option in _options)
        {
            menu.Add(option, () =>
            {
                _actions[_options.IndexOf(option)]();
            });
        }
        menu.Configure(config =>
        {
            config.Selector = "--> ";
            config.Title = "Menu";
            config.EnableBreadcrumb = true;
            config.WriteBreadcrumbAction = titles => Console.WriteLine(string.Join(" / ", titles));
        });

        menu.Show();
    }

}