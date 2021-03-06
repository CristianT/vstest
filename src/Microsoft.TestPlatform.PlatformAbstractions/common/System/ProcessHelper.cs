// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.TestPlatform.PlatformAbstractions
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;

    using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;

    /// <summary>
    /// Helper class to deal with process related functionality.
    /// </summary>
    public class ProcessHelper : IProcessHelper
    {
        /// <inheritdoc/>
        public object LaunchProcess(string processPath, string arguments, string workingDirectory, IDictionary<string, string> envVariables, Action<object, string> errorCallback, Action<object> exitCallBack)
        {
            var process = new Process();
            try
            {
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.WorkingDirectory = workingDirectory;

                process.StartInfo.FileName = processPath;
                process.StartInfo.Arguments = arguments;
                process.StartInfo.RedirectStandardError = true;
                process.EnableRaisingEvents = true;

                if (envVariables != null)
                {
                    foreach (var kvp in envVariables)
                    {
                        process.StartInfo.Environment.Add(kvp.Key, kvp.Value);
                    }
                }

                if (errorCallback != null)
                {
                    process.ErrorDataReceived += (sender, args) => errorCallback(sender as Process, args.Data);
                }

                if (exitCallBack != null)
                {
                    process.Exited += (sender, args) =>
                    {
                        // Call WaitForExit again to ensure all streams are flushed
                        var p = sender as Process;
                        p.WaitForExit();
                        exitCallBack(p);
                    };
                }

                // EqtTrace.Verbose("ProcessHelper: Starting process '{0}' with command line '{1}'", processPath, arguments);
                process.Start();

                if (errorCallback != null)
                {
                    process.BeginErrorReadLine();
                }
            }
            catch (Exception)
            {
                process.Dispose();
                process = null;

                // EqtTrace.Error("TestHost Object {0} failed to launch with the following exception: {1}", processPath, exception.Message);

                throw;
            }

            return process as object;
        }

        /// <inheritdoc/>
        public string GetCurrentProcessFileName()
        {
            return Process.GetCurrentProcess().MainModule.FileName;
        }

        /// <inheritdoc/>
        public string GetTestEngineDirectory()
        {
            return Path.GetDirectoryName(typeof(ProcessHelper).GetTypeInfo().Assembly.Location);
        }

        /// <inheritdoc/>
        public int GetCurrentProcessId()
        {
            return Process.GetCurrentProcess().Id;
        }

        /// <inheritdoc/>
        public string GetProcessName(int processId)
        {
            return Process.GetProcessById(processId).ProcessName;
        }

        /// <inheritdoc/>
        public bool TryGetExitCode(object process, out int exitCode)
        {
            var proc = process as Process;
            if (proc != null && proc.HasExited)
            {
                exitCode = proc.ExitCode;
                return true;
            }

            exitCode = 0;
            return false;
        }

        /// <inheritdoc/>
        public void SetExitCallback(int processId, Action callbackAction)
        {
            var process = Process.GetProcessById(processId);

            process.EnableRaisingEvents = true;
            process.Exited += (sender, args) =>
                {
                    callbackAction.Invoke();
                };
        }

        /// <inheritdoc/>
        public void TerminateProcess(int processId)
        {
            var process = Process.GetProcessById(processId);
            process.Kill();
        }
    }
}
