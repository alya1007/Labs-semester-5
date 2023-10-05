using TMPS.Domain.Interfaces;

namespace TMPS.Domain.Models.Reports;

public class Report : IPrototype<Report>
{
    public string Title { get; set; }
    public string Content { get; set; }
    public string Footer { get; set; }

    public Report(string title, string content, string footer)
    {
        Title = title;
        Content = content;
        Footer = footer;
    }

    public void Print()
    {
        Console.WriteLine("Report: ");
        Console.WriteLine($"Title: {Title}");
        Console.WriteLine($"Content: {Content}");
        Console.WriteLine($"Footer: {Footer}");
    }
    public Report Clone() => (Report)MemberwiseClone();

}