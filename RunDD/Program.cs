using System;
using System.Configuration;

namespace RunDD
{
    internal struct Paths
    {
        public string filePath { get; set; }
        public string reportPath { get; set; }
        public string symbolPath { get; set; }

        public Paths(string fp, string rp, string sp)
        {
            filePath = fp;
            reportPath = rp;
            symbolPath = sp;
        }
    }

    internal class Program
    {

        private static AnalysisJob InitializeJob(Paths paths)
        {
            if (String.IsNullOrEmpty(paths.filePath))
            {
                Console.WriteLine($"The path to the dump file should be specified!");
                return null;
            }

            if (String.IsNullOrEmpty(paths.reportPath))
            {
                Console.WriteLine($"The report file path is empty. Using {Environment.CurrentDirectory}");
                paths.reportPath = Environment.CurrentDirectory;
            }

            if (String.IsNullOrEmpty(paths.symbolPath))
            {
                paths.symbolPath = ConfigurationManager.AppSettings["SymbolPath"].ToString();
                Console.WriteLine($"The symbol file path is empty. Using {paths.symbolPath}");
            }

            var strRulePath = ConfigurationManager.AppSettings["AnalysisRulePath"].ToString();


            return new AnalysisJob(paths.filePath, paths.symbolPath, paths.reportPath, strRulePath);
        }

        private static AnalysisJob Initialize(Paths paths)
        {

            return InitializeJob(paths);
        }

        private static void DisplayUsage()
        {
            Console.WriteLine();

        }

        private static Paths ProcessArguments(string[] args)
        {
            string filePath = "", reportPath = "", symbolPath = "";

            for (int i = 0; i != args.Length; i++)
            {
                switch (args[i].Trim())
                {
                    case "-f":
                        filePath = args[i + 1];
                        break;
                    case "-r":
                        reportPath = args[i + 1];
                        break;
                    case "-s":
                        symbolPath = args[i + 1];
                        break;
                    case "-h":
                        DisplayUsage();
                        break;
                    case "-?":
                        DisplayUsage();
                        break;
                    default:
                        break;
                }
            }

            return new Paths(filePath, reportPath, symbolPath);

        }

        private static void Analyze(AnalysisJob aj)
        {
            aj.StartJob();
        }

        static void Main(string[] args)
        {
            Analyze(Initialize(ProcessArguments(args)));
        }
    }
}
