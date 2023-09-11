using EnchCoreApi.Common.CSharp.MSBuild;
using EnchCoreApi.Common.CSharp.MSBuild.Platform;
using Microsoft.Build.Evaluation;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace EnchCoreApi.Common.Compiler
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var server = new Server();
            server.Run();
        }
        static void Old_Main(string[] args)
        {
            try
            {
                CompilePlatform platform = new CompilePlatform(false);

                var param = new CompileParameter();
                //param.Deseriailze(args);
                var project = platform.CreateProject(param.ProjectPath);
                project.LoadFromExistingFile();
                var logger = platform.CreateCompileLogger(param.LoggerDirectory);
                project.Build(logger);
                if (project.Builded)
                {
                    var file = project.OutPutPath;
                    Console.WriteLine($"{project.ProjectFile.Name} 编译完成 输出：{file}");
                    //return CompileResult.Success;
                }
                if (logger.CompilerErrors.Count > 0)
                {
                    var text = $"编译失败,以下为错误代码:\n";

                    Dictionary<string, string[]> cache = new Dictionary<string, string[]>();
                    foreach (var compErr in logger.CompilerErrors)
                    {
                        if (!File.Exists(compErr.FileName))
                        {
                            text += $"程序集错误：{compErr.ErrorText}\n\n";
                            continue;
                        }
                        else
                        {
                            if (!cache.TryGetValue(compErr.FileName, out var code))
                            {
                                code = File.ReadAllLines(compErr.FileName);
                                cache.Add(compErr.FileName, code);
                            }
                        }
                        var codes = cache[compErr.FileName];
                        if (compErr.Line >= codes.Length || compErr.Line <= 0)
                        {
                            text += $"程序集错误：{compErr.ErrorText}\n\n";
                            continue;
                        }
                        text += $"●文件：{compErr.FileName}\n";
                        text += $"●行号：{compErr.Line}错误：{compErr.ErrorText}\n";
                        text += $"●代码：{codes[compErr.Line - 1].TrimStart()}\n\n";
                    }
                    Console.WriteLine(text);
                }
                else
                {
                    var text = $"编译失败，可查看日志追踪问题：{logger.CurrentPath}";
                }
                //return CompileResult.Failure;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                // CompileResult.Failure;
            }
        }
    }
}