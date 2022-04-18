using CommandLine;
using CommandLine.Text;
using System;

namespace CheckIP.CLI
{
    internal class Program
    {
        public class Options
        {
            [Usage(ApplicationAlias = "CheckIP")]

            [Value(index: 0, Required = true, HelpText = "Image file Path to analyze.")]
            public string Path { get; set; }
            
            [Option(shortName: 'c', longName: "confidence", Required = false, HelpText = "Minimum confidence.", Default = 0.9f)]
            public float Confidence { get; set; }
        }

        static void Main(string[] args)
        {
            CommandLine.Parser.Default.ParseArguments<Options>(args)
                .WithParsed<Options>(o => {
                // parsing successful; go ahead and run the app
                })
                .WithNotParsed<Options>(e => {
                // parsing unsuccessful; deal with parsing errors
                });
            Console.WriteLine("Hello World.");
        }
    }
}