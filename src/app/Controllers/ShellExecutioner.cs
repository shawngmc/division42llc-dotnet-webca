using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace app.Controllers
{
    public class ShellExecutioner
    {
        // From: http://stackoverflow.com/questions/285760/how-to-spawn-a-process-and-capture-its-stdout-in-net
        public static String ShellExecute(String path, String command, String arguments, 
            TextWriter writerAll, TextWriter writerStdOut, TextWriter writerStdErr)
        {
            var startInfo = new ProcessStartInfo
            {
                WorkingDirectory = path,
                FileName = command,
                Arguments = arguments,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            using (var process = Process.Start(startInfo))
            {
                using (process.StandardOutput)
                {
                    writerAll?.WriteLine(process.StandardOutput.ReadToEnd());
                    writerStdOut?.WriteLine(process.StandardOutput.ReadToEnd());
                }
                using (process.StandardError)
                {
                    writerAll?.WriteLine(process.StandardOutput.ReadToEnd());
                    writerStdErr?.WriteLine(process.StandardError.ReadToEnd());
                }
            }

            return path;
        }
    }
}