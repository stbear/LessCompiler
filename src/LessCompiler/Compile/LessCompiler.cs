﻿using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace LessCompiler
{
    class LessCompiler : ICompiler
    {
        private static Regex _errorRx = new Regex("(?<message>.+) on line (?<line>[0-9]+), column (?<column>[0-9]+)", RegexOptions.Compiled);
        private readonly string _path;
        private string _output = string.Empty;
        private string _error = string.Empty;

        public LessCompiler(string path)
        {
            _path = path;
        }

        public CompilerResult Compile(Config config)
        {
            string baseFolder = Path.GetDirectoryName(config.FileName);
            string inputFile = Path.Combine(baseFolder, config.InputFile);

            FileInfo info = new FileInfo(inputFile);
            string content = File.ReadAllText(info.FullName);

            CompilerResult result = new CompilerResult
            {
                FileName = info.FullName,
                OriginalContent = content,
            };

            try
            {
                RunCompilerProcess(config, info);

                result.CompiledContent = _output;

                if (_error.Length > 0)
                {
                    CompilerError ce = new CompilerError
                    {
                        FileName = info.FullName,
                        Message = _error.Replace(baseFolder, string.Empty),
                        IsWarning = !string.IsNullOrEmpty(_output)
                    };

                    var match = _errorRx.Match(_error);

                    if (match.Success)
                    {
                        ce.Message = match.Groups["message"].Value.Replace(baseFolder, string.Empty);
                        ce.LineNumber = int.Parse(match.Groups["line"].Value);
                        ce.ColumnNumber = int.Parse(match.Groups["column"].Value);
                    }

                    result.Errors.Add(ce);
                }
            }
            catch (Exception ex)
            {
                CompilerError error = new CompilerError
                {
                    FileName = info.FullName,
                    Message = string.IsNullOrEmpty(_error) ? ex.Message : _error,
                    LineNumber = 0,
                    ColumnNumber = 0,
                };

                result.Errors.Add(error);
            }

            return result;
        }

        private void RunCompilerProcess(Config config, FileInfo info)
        {
            string arguments = ConstructArguments(config);
            string slash = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "\\" : "/";
            ProcessStartInfo start = new ProcessStartInfo
            {
                WorkingDirectory = info.Directory.FullName,
                UseShellExecute = false,
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true,
                FileName = "node",
                Arguments = $"\"{Path.Combine(_path, $"node_modules{slash}less{slash}bin{slash}lessc")}\" {arguments} \"{info.FullName}\"",
                StandardOutputEncoding = Encoding.UTF8,
                StandardErrorEncoding = Encoding.UTF8,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
            };

            start.EnvironmentVariables["PATH"] = _path + ";" + start.EnvironmentVariables["PATH"];
            start.EnvironmentVariables["NODE_SKIP_PLATFORM_CHECK"] = "1";

            using (Process p = Process.Start(start))
            {
                var stdout = p.StandardOutput.ReadToEndAsync();
                var stderr = p.StandardError.ReadToEndAsync();
                p.WaitForExit();

                _output = stdout.Result.Trim();
                _error = stderr.Result.Trim();
            }
        }

        private static string ConstructArguments(Config config)
        {
            string arguments = " --no-color --js";

            LessOptions options = LessOptions.FromConfig(config);

            if (options.Math != null)
                arguments += $" --math={options.Math}";

            //if (options.IECompat)
            //arguments += " --ie-compat";

            if (options.StrictUnits)
                arguments += " --strict-units=on";

            if (options.RelativeUrls)
                arguments += " --rewrite-urls=all";

            if (!string.IsNullOrEmpty(options.RootPath))
                arguments += $" --rootpath=\"{options.RootPath}\"";

            if (!string.IsNullOrEmpty(options.AutoPrefix))
                arguments += $" --autoprefix=\"{options.AutoPrefix}\"";

            if (!string.IsNullOrEmpty(options.CssComb) && !options.CssComb.Equals("none", StringComparison.OrdinalIgnoreCase))
                arguments += $" --csscomb=\"{options.CssComb}\"";

            if (options.SourceMapOptions != null)
            {
                if (!string.IsNullOrEmpty(options.SourceMapOptions.OutputFilename))
                    arguments += " --source-map=" + options.SourceMapOptions.OutputFilename;

                if (!string.IsNullOrEmpty(options.SourceMapOptions.SourceMapRootpath))
                    arguments += " --source-map-rootpath=" + options.SourceMapOptions.SourceMapRootpath;

                if (!string.IsNullOrEmpty(options.SourceMapOptions.SourceMapBasepath))
                    arguments += " --source-map-basepath=" + options.SourceMapOptions.SourceMapBasepath;

                if (options.SourceMapOptions.OutputSourceFiles)
                    arguments += " --source-map-include-source";

                if (options.SourceMapOptions.SourceMapFileInline)
                    arguments += " --source-map-map-inline";

                if (!string.IsNullOrEmpty(options.SourceMapOptions.SourceMapURL))
                    arguments += " --source-map-url=" + options.SourceMapOptions.SourceMapURL;
            }
            else if (options.SourceMap) // WebCompiler compatibility
                arguments += " --source-map-map-inline";

            return arguments;
        }
    }
}
