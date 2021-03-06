// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Interface for any process related functionality. This is needed for clean unit-testing.
    /// </summary>
    public interface IProcessHelper
    {
        /// <summary>
        /// Launches the process with the given arguments.
        /// </summary>
        /// <param name="processPath">The full file name of the process.</param>
        /// <param name="arguments">The command-line arguments.</param>
        /// <param name="workingDirectory">The working directory for this process.</param>
        /// <param name="environmentVariables">Environment variables to set while bootstrapping the process.</param>
        /// <param name="errorCallback">Call back for to read error stream data</param>
        /// <param name="exitCallBack">Call back for on process exit</param>
        /// <returns>The process created.</returns>
        object LaunchProcess(string processPath, string arguments, string workingDirectory, IDictionary<string, string> environmentVariables, Action<object, string> errorCallback, Action<object> exitCallBack);

        /// <summary>
        /// Gets the current process file path.
        /// </summary>
        /// <returns>The current process file path.</returns>
        string GetCurrentProcessFileName();

        /// <summary>
        /// Gets the location of test engine.
        /// </summary>
        /// <returns>Location of test engine.</returns>
        string GetTestEngineDirectory();

        /// <summary>
        /// Gets the process id of test engine.
        /// </summary>
        /// <returns>process id of test engine.</returns>
        int GetCurrentProcessId();

        /// <summary>
        /// Gets the process name for given process id.
        /// </summary>
        /// <param name="processId">process id</param>
        /// <returns>Name of process</returns>
        string GetProcessName(int processId);

        /// <summary>
        /// False if process has not exited, True otherwise. Set exitCode only if process has exited.
        /// </summary>
        /// <param name="process">process parameter</param>
        /// <param name="exitCode">return value of exitCode</param>
        /// <returns>False if process has not exited, True otherwise</returns>
        bool TryGetExitCode(object process, out int exitCode);

        /// <summary>
        /// Sets the process exit callback.
        /// </summary>
        /// <param name="processId">
        /// The process id.
        /// </param>
        /// <param name="callbackAction">
        /// Callback on process exit.
        /// </param>
        void SetExitCallback(int processId, Action callbackAction);

        /// <summary>
        /// Terminates a process.
        /// </summary>
        /// <param name="processId">Id of the process to terminate.</param>
        void TerminateProcess(int processId);
    }
}
