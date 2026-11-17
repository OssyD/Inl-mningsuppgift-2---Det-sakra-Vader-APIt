using Spectre.Console;

namespace WeatherApp.App.Helpers;

public static class ConsoleHelper
{
    public static void ShowHeader()
    {
        AnsiConsole.Write(new Rule("[bold deepskyblue1]FAILSAFE WEATHERMONITOR[/]"));
    }

    public static void ShowWarning(string message)
    {
        AnsiConsole.MarkupLine($"[yellow]{Markup.Escape(message)}[/]");
    }

    public static void ShowError(string message)
    {
        AnsiConsole.MarkupLine($"[red]{Markup.Escape(message)}[/]");
    }

    public static void WaitForKey()
    {
        AnsiConsole.MarkupLine("\n[grey]Press any key to continue...[/]");
        Console.ReadKey(true);
    }
}
