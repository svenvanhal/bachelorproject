﻿using Timetabling.Exceptions;
using Timetabling.Resources;

namespace Timetabling.Algorithms
{

    /// <summary>
    /// Timetable generation algorithm interface.
    /// </summary>
    public abstract class Algorithm
    {

        /// <summary>
        /// Initialize and execute algorithm, return results.
        /// </summary>
        /// <param name="identifier">Unique identifier for this algorithm run.</param>
        /// <param name="input">Input to run the algorithm on.</param>
        /// <returns>A Timetable object.</returns>
        public abstract Timetable Execute(string identifier, string input);

        /// <summary>
        /// Algorithm initialization phase.
        /// </summary>
        /// <param name="input">Input to run the algorithm on.</param>
        protected abstract void Initialize(string input);

        /// <summary>
        /// Runs the algorithm on a data set. Implementation should be asynchronous.
        /// 
        /// TODO: change string to DataRepository input argument, generate FET inputfile via FetAlgorithm class.
        /// </summary>
        /// <exception cref="AlgorithmException">Thrown when an error occurred during algorithm execution.</exception>
        protected abstract void Run();

        /// <summary>
        /// Generates a <see cref="Timetable"/> from the algorithm output.
        /// </summary>
        /// <returns>A Timetable object.</returns>
        protected abstract Timetable GetResult();
    }
}
