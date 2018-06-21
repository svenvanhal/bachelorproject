﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Timetabling.Algorithms;
using Timetabling.DB;
using Timetabling.Objects;

namespace Timetabling
{

    /// <summary>
    /// Program entrypoint.
    /// </summary>
    public class TimetableGenerator : IDisposable
    {

        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Unique string to identify the current run of an algorithm.
        /// </summary>
        public string CurrentRunIdentifier { get; private set; }

        /// <summary>
        /// Cancellation token source.
        /// </summary>
        public CancellationTokenSource TokenSource;

        /// <inheritdoc />
        public TimetableGenerator(CancellationTokenSource source = null)
        {
            TokenSource = source ?? new CancellationTokenSource();
        }

        /// <summary>
        /// Run an algorithm on an input file.
        /// </summary>
        public Task<Timetable> RunAlgorithm(TimetablingStrategy strategy, DataModel model)
        {
            Logger.Info($"Starting {strategy.GetType().Name} algorithm run");

            // Generate new ID for this algorithm run
            RefreshIdentifier();

            // Generate timetable
            return strategy.GenerateTask(CurrentRunIdentifier, model, TokenSource.Token);
        }

        /// <summary>
        /// Stops the algorithm that is currently running.
        /// </summary>
        public void StopAlgorithm()
        {
            Logger.Info("Terminating algorithm");
            TokenSource.Cancel();
        }

        /// <summary>
        /// Generates a new identifier.
        /// </summary>
        public void RefreshIdentifier()
        {
            CurrentRunIdentifier = Guid.NewGuid().ToString("B");
            Logger.Info($"Generated new identifier - {CurrentRunIdentifier}");
        }

        /// <inheritdoc />
        public void Dispose()
        {
            TokenSource?.Dispose();
            TokenSource = null;
        }
    }
}
