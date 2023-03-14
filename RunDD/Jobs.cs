using DebugDiag.DotNet;
using DebugDiag.DotNet.AnalysisRules;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace RunDD
{
    internal class AnalysisJobBase
    {

       
        public string FileName { get; set; }
        public string ReportFileName { get;  set; }
        //public List<string> FileNames { get; set; }

        //private string ComputerName { get; set; }
        //public byte[] ReportFileContent { get; set; }

        public AnalysisJobBase()
        {
        }

        public virtual void Analyze(object Data)
        {
            return;
        }

        public virtual void Initialize()
        {
            return;
        }
    }

    internal class AnalysisJob : AnalysisJobBase
    {

        private string SymbolPath { get; set; }
        private string ReportingPath { get; set; }
        private string AnalysisRulePath { get; set; }
        //private LogWriter Logger { get; set; }        

        public AnalysisJob(string dumpPath, string symPath, string rptPath, string anlysRulePath)
        {
            FileName = dumpPath;
            SymbolPath = symPath;
            ReportingPath = rptPath;
            AnalysisRulePath = anlysRulePath;
     
        }

        public void StartJob()
        {
            Thread thread = null;

            try
            {
                thread = new Thread(new ParameterizedThreadStart(Analyze));
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start(null);
                thread.Join();
            }
            catch (Exception ex)
            {                
                Console.WriteLine($"{Properties.Resource.EC200} {ex.Message}");
            }

        }

        public override void Analyze(object Data)
        {
            bool IncludeSourceAndLineInformationInAnalysisReports = false;
            bool SetContextOnCrashDumps = false;
            bool DoHangAnalysisOnCrashDumps = false;
            bool IncludeHttpHeadersInClientConns = false;
            bool IncludeInstructionPointerInAnalysisReports = false;
            bool ExcludeIdenticalStacks = false;
            List<string> df = new List<string>();

            var o = Console.Out;

            using (NetAnalyzer netAnalyzer = new NetAnalyzer())
            {
                try
                {
                    Console.SetOut(TextWriter.Null);
                    netAnalyzer.Initialize(IncludeSourceAndLineInformationInAnalysisReports, SetContextOnCrashDumps, DoHangAnalysisOnCrashDumps, IncludeHttpHeadersInClientConns, ExcludeIdenticalStacks, IncludeInstructionPointerInAnalysisReports);
                    df.Add(FileName);
                    netAnalyzer.AddDumpFiles(df, SymbolPath);
                    netAnalyzer.AddAnalysisRulesToRunList(AnalysisRulePath, false);
                    netAnalyzer.RunAnalysisRules(null, SymbolPath, "", ReportingPath, AnalysisModes.Unattended);                    
                    
                    while (!netAnalyzer.ReportReady)
                        System.Threading.Thread.Sleep(1000);

                    // We need to understand when netanalyzer produces multiple reports. Currently we rely on the first non-null report name !!!
                    for (int i = 0; i != netAnalyzer.ReportFileNames.GetLength(0); i++)
                        if (netAnalyzer.ReportFileNames[i] != null)
                            ReportFileName = netAnalyzer.ReportFileNames[i];

                    Console.SetOut(o);
                }
                catch (Exception ex)
                {
                    Console.SetOut(o);
                    Console.WriteLine($"{Properties.Resource.EC201} {ex.Message}");
                }
            }

        }

    }
}
