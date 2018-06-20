﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Timetabling.DB;
using Timetabling.Objects;

namespace Timetabling.Algorithms.FET
{

    /// <inheritdoc />
    /// <summary>
    /// The FET-CL wrapper. Visit the <a href="https://lalescu.ro/liviu/fet/">official FET website</a> for more information about the program and the algorithm.
    /// </summary>
    public class FetAlgorithm : TimetablingStrategy
    {

        /// <summary>
        /// Current run identifier.
        /// </summary>
        protected internal string Identifier { get; set; }

        /// <summary>
        /// Name of the input file (filename without extension). FET-CL uses this as base for all generated output files.
        /// </summary>
        protected internal string InputName { get; set; }

        /// <summary>
        /// Path to the .fet input file.
        /// </summary>
        protected internal string InputFile
        {
            get => _inputFile;
            set
            {
                _inputFile = value;
                InputName = Path.GetFileNameWithoutExtension(value);
            }
        }

        /// <summary>
        /// Output directory.
        /// </summary>
        protected internal string OutputDir { get; set; }

        /// <summary>
        /// Activities to be scheduled.
        /// </summary>
        protected internal IDictionary<int, Activity> Activities { get; set; }

        /// <summary>
        /// Facade to interact with the FET-CL process.
        /// </summary>
        internal FetProcessFacade ProcessFacade;

        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private string _inputFile;
        private TaskCompletionSource<Timetable> _tcs;

        /// <inheritdoc />
        protected internal override Task<Timetable> GenerateTask(string identifier, DataModel input, CancellationToken t)
        {
            Identifier = identifier;
            _tcs = new TaskCompletionSource<Timetable>(t);

            // Initialize algorithm
            Initialize(input, t);

            // Create algorithm task
            ProcessFacade.StartProcess().ContinueWith(task =>
            {
                // Bubble exception(s) if faulted
                if (task.IsFaulted) _tcs.SetException(task.Exception.InnerExceptions);
                else if (task.IsCanceled) _tcs.SetCanceled();

                // Try and get result on success
                else _tcs.SetResult(GetResult(Activities));
            });

            return _tcs.Task;
        }

        /// <summary>
        /// Initialize the algorithm by creating a FET-CL process.
        /// </summary>
        /// <param name="model">Data model to generate the input file from</param>
        /// <param name="t">Cancellation token</param>
        protected internal void Initialize(DataModel model, CancellationToken t)
        {
            Logger.Info("Initializing FET algorithm");

            var generator = new FetInputGenerator(model);

            // Set parameters
            OutputDir = CreateOutputDirectory(Identifier);
            InputFile = generator.GenerateFetFile(OutputDir);

            // Store activities
            Activities = generator.GetActivities();

            // Create process interface and register exit handler
            ProcessFacade = new FetProcessFacade(CreateProcess(), t);
        }

        /// <summary>
        /// Retrieve the timetable from the FET algorithm output.
        /// </summary>
        /// <returns>Timetable object</returns>
        protected internal Timetable GetResult(IDictionary<int,Activity> activities)
        {
            Logger.Info("Retrieving FET algorithm results");

            var outputProcessor = new FetOutputProcessor(InputName, Path.Combine(OutputDir, "timetables"));
            return outputProcessor.GetTimetable(activities);
        }

        /// <summary>
        /// Configure and create a new FET-CL process.
        /// </summary>
        /// <returns></returns>
        protected internal Process CreateProcess()
        {
            var fetExecutablePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, FetConfig.GetSetting("FetExecutableLocation"));
            var processBuilder = new FetProcessBuilder(fetExecutablePath);

            // Set input and output dir
            processBuilder.SetInputFile(InputFile);
            processBuilder.SetOutputDir(OutputDir);

            // Change default language
            processBuilder.SetLanguage(FetConfig.GetFetLanguage());

            // Set timeout
            processBuilder.SetTimeout(FetConfig.GetTimeout());

            return processBuilder.CreateProcess();
        }

        /// <summary>
        /// Creates a temporary directory in the to store the intermediary algorithm output.
        /// The working directory can be changed in app.config.
        /// </summary>
        /// <param name="outputDir">Name of the subdirectory.</param>
        /// <returns>Full path to the output directory.</returns>
        protected internal static string CreateOutputDirectory(string outputDir)
        {
            // Get working dir
            var workingDir = FetConfig.GetFetWorkingDir();

            // Create new directory and return path
            var dir = Directory.CreateDirectory(Path.Combine(workingDir, outputDir));
            return dir.FullName;
        }

    }
}
