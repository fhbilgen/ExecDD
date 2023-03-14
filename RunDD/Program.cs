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
                Console.WriteLine($"{Properties.Resource.EC020}");
                return null;
            }

            if (String.IsNullOrEmpty(paths.reportPath))
            {
                Console.WriteLine($"{Properties.Resource.EC021} {Environment.CurrentDirectory}");
                paths.reportPath = Environment.CurrentDirectory;
            }

            if (String.IsNullOrEmpty(paths.symbolPath))
            {
                paths.symbolPath = ConfigurationManager.AppSettings["SymbolPath"].ToString();
                Console.WriteLine($"{Properties.Resource.EC022} {paths.symbolPath}");
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
            Console.WriteLine($"\n{Properties.Resource.EC001}");
            Console.WriteLine($"{Properties.Resource.EC002}\n");
            Console.WriteLine($"{Properties.Resource.EC003}\n");
            Console.WriteLine($"{Properties.Resource.EC004}\n");
            Console.WriteLine($"{Properties.Resource.EC005}\n");            
            Console.WriteLine($"{Properties.Resource.EC006}");
            Console.WriteLine($"{Properties.Resource.EC007}\n");
            Console.WriteLine($"{Properties.Resource.EC008}");
            Console.WriteLine($"{Properties.Resource.EC009}");
            Console.WriteLine("\n\n");

            Console.WriteLine($"{Properties.Resource.EC010}");
            Console.WriteLine($"{Properties.Resource.EC011}\n");


            Environment.Exit(0);
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
