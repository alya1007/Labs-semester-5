using ConsoleTools;

namespace TMPS.Common;

public class MenuHandler
{
    private readonly List<string> _titles;
    private readonly List<Action> _commands;

    public MenuHandler(List<string> titles, List<Action> commands)
    {
        _titles = titles;
        _commands = commands;
    }

    public void ShowMenu()
    {
        var menu = new ConsoleMenu();
        foreach (var option in _titles)
        {
            menu.Add(option, () =>
            {
                _commands[_titles.IndexOf(option)]();
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