# ExecDD
RunDD v1.0 Run DebugDiag
Command line application to invoke the DebugDiag Analysis utility

Usage:

RunDD.exe <-f> | [-r] | [-s]

-f name and path of the dump file

-r name and path of the output report file.
   If not specified then the report will be generated in the folder of RunDD.exe

-s symbol path
   If not specified then the value of SymbolPath in RunDD.exe.config is used



RunDD.exe assumes that DebugDiag installation path is C:\Program Files\DebugDiag\
If not then please change the value of AnalysisRulePath in RunDD.exe.config file
