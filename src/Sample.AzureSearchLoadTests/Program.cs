using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using System.Threading.Tasks;

namespace Sample.ElasticLoadTests
{
    public class Program
    {
        async static Task Main(string[] args)
        {
            var main = new RootCommand
            {
                CreateRunCommand()
            };

            await main.InvokeAsync(args);
        }

        private static Command CreateRunCommand()
            => new Command("run", "Run Elastic load test")
            {
                new Option<int>(
                    new [] { "--iterationCount", "-i"},
                    () => 600,
                    "How many requests to send to search.")
            }.WithHandler(CommandHandler.Create<int>(LoadTest.RunAsync));
    }
}
