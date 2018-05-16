﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using Timetabling.Exceptions;
using Timetabling.Helper;
using Timetabling.Resources;

namespace Timetabling.Algorithms
{

    /// <summary>
    /// The FET-CL wrapper. Visit the <a href="https://lalescu.ro/liviu/fet/">official FET website</a> for more information about the program and the algorithm.
    /// </summary>
    public class FetAlgorithm : Algorithm
    {

        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Location of the FET program.
        /// </summary>
        private readonly string executableLocation;

        /// <summary>
        /// FET-CL command line arguments.
        /// </summary>
        private readonly CommandLineArguments args;

        /// <summary>
        /// Algorithm input file.
        /// </summary>
        private string inputFile;

        /// <summary>
        /// Algorithm output directory.
        /// </summary>
        private string outputDir;

        /// <summary>
        /// Unique string to identify the current run of the algorithm. The FET output is stored using this identifier.
        /// </summary>
        public string CurrentRunIdentifier { get; private set; }

        /// <summary>
        /// Instantiate new FET Algorithm instance.
        /// <param name="executableLocation">Location of the FET binary.</param>
        /// <param name="args">Additional command line arguments.</param>
        /// </summary>
        public FetAlgorithm(string executableLocation, CommandLineArguments args = null)
        {
            this.executableLocation = executableLocation;
            this.args = args ?? new CommandLineArguments();
        }

        /// <summary>
        /// Set a FET-CL command line argument.
        /// </summary>
        /// <param name="name">Name of the argument.</param>
        /// <param name="value">Value of the argument. If null, the argument is removed.</param>
        public void SetArgument(string name, string value)
        {
            if (value == null) args.Remove(name);
            else args[name] = value;
        }

        /// <summary>
        /// Get a FET-CL command line argument.
        /// </summary>
        /// <param name="name">Name of the argument.</param>
        /// <return>Value of the argument. Null if argument not set.</return>
        /// <exception cref="KeyNotFoundException">Throws exception if <paramref name="name"/> not found.</exception>
        public string GetArgument(string name)
        {
            return args[name];
        }

        /// <inheritdoc />
        public override Timetable Execute(string inputFile)
        {

            Initialize(inputFile);
            Run();
            return GetResult();

        }

        /// <summary>
        /// Defines a new input file for the algorithm.
        /// </summary>
        /// <param name="inputFileLocation">Location of the FET input data file.</param>
        protected override void Initialize(string inputFileLocation)
        {

            Logger.Info("Initializing FET algorithm");

            // Create unique identifier
            RefreshIdentifier();

            inputFile = inputFileLocation;
            SetArgument("inputfile", inputFile);

            outputDir = Util.CreateTempFolder(CurrentRunIdentifier);
            SetArgument("outputdir", outputDir);
        }

        /// <summary>
        /// Executes the FET algorithm.
        /// </summary>
        /// <exception cref="AlgorithmException">Error message during algorithm execution.</exception>
        protected override void Run()
        {

            Logger.Info("Running FET algorithm");

            // Create new FET process
            var fetProcess = CreateProcess();

            try
            {

                Logger.Info("Starting FET process");

                // Run the FET program
                fetProcess.Start();
                fetProcess.BeginOutputReadLine();
                fetProcess.WaitForExit();

                // Verify that FET executed successfully
                CheckProcessExitCode(fetProcess.ExitCode);

            }
            catch (Exception ex)
            {
                throw new AlgorithmException("Could not execute FET algorithm.", ex);
            }
            finally
            {
                fetProcess.Dispose();
            }

        }

        /// <summary>
        /// Fetches the FET output files and generates a Timetable object.
        /// </summary>
        /// <returns>A Timetable object.</returns>
        protected override Timetable GetResult()
        {

            Logger.Info("Retrieving FET algorithm results");

            return new Timetable();
        }

        /// <summary>
        /// Set-up process info for the FET program.
        /// </summary>
        /// <returns>ProcessStartInfo object</returns>
        private Process CreateProcess()
        {

            Logger.Info("Creating FET process");

            // Default arguments
            var defaults = new CommandLineArguments
            {
                { "verbose", "true" }
            };

            var startInfo = new ProcessStartInfo
            {

                // Hide window
                UseShellExecute = false,

                // Set executable location and arguments
                FileName = executableLocation,
                Arguments = defaults.Combine(args).ToString(),

                // Redirect stdout and stderr
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            var fetProcess = new Process
            {
                StartInfo = startInfo,
                EnableRaisingEvents = true
            };

            // Add listeners
            fetProcess.OutputDataReceived += LogConsoleOutput;

            Logger.Debug("Process arguments: " + startInfo.Arguments);

            return fetProcess;
        }

        /// <summary>
        /// Generates a new identifier.
        /// </summary>
        private void RefreshIdentifier()
        {
            CurrentRunIdentifier = Guid.NewGuid().ToString("B");
        }

        /// <summary>
        /// Logs FET console output line.
        /// </summary>
        /// <param name="sender">Originating process.</param>
        /// <param name="eventArgs">Event data.</param>
        private static void LogConsoleOutput(object sender, DataReceivedEventArgs eventArgs)
        {

            var data = eventArgs.Data;
            if (!string.IsNullOrWhiteSpace(data))
            {
                Logger.Debug(data);
            }

        }

        /// <summary>
        /// Checks the FET process exit code and throws an exception if the exit code is non-zero.
        /// </summary>
        /// <param name="exitCode">The exit code of a process.</param>
        /// <exception cref="AlgorithmException">Throws AlgorithmException if non-zero error code.</exception>
        private static void CheckProcessExitCode(int exitCode)
        {
            if (exitCode != 0) throw new AlgorithmException($"The FET process has exited with a non-zero exit code ({exitCode}).");
        }

    }
}
