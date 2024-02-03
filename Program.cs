
using Letu.ChatPdf.Cli;
using System.CommandLine;

public class Program
{
    public static async Task Main(string[] args)
    {
        var rootCommand = new RootCommand("大语言模型实例-Embedding");
        rootCommand.AddCommand(InitCommand.Create());
        rootCommand.AddCommand(QueryCommand.Create());
        await rootCommand.InvokeAsync(args);
    }
}