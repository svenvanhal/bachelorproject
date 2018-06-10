using System.Threading;
using System.Threading.Tasks;
using Timetabling.Resources;

namespace Timetabling.Algorithms
{

    /// <summary>
    /// Task-based timetable generation.
    /// </summary>
    public abstract class Algorithm
    {

        /// <summary>
        /// Generate Task which executes the algorithm on a given resource collection.
        /// </summary>
        /// <param name="identifier">Unique identifier for this algorithm run.</param>
        /// <param name="input">Input to run the algorithm on.</param>
        /// <param name="t">Cancellation token.</param>
        /// <returns>A Task-object yielding a Timetable.</returns>
        protected internal abstract Task<Timetable> GenerateTask(string identifier, TimetableResourceCollection input, CancellationToken t);

    }
}
