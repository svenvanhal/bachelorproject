using System;
using System.Collections.Generic;
using Timetabling.Resources.Constraints;

namespace Timetabling.Resources
{

    /// <summary>
    /// Stores all resources required for timetable generation.
    /// </summary>
    public class TimetableResourceCollection
    {

        /// <summary>
        /// Available days for the timetable.
        /// </summary>
        public Dictionary<int, Day> Days { get; set; }

        /// <summary>
        /// Available periods for the timetable.
        /// </summary>
        public Dictionary<int, Timeslot> Timeslots { get; set; }

        /// <summary>
        /// Available subjects for the timetable.
        /// </summary>
        public Dictionary<int, Subject> Subjects { get; set; }

        /// <summary>
        /// Available teachers for the timetable.
        /// </summary>
        public Dictionary<int, Teacher> Teachers { get; set; }

        /// <summary>
        /// Available rooms for the timetable.
        /// </summary>
        public Dictionary<int, StudentSet> Students { get; set; }

        /// <summary>
        /// The activities to be scheduled in the timetable.
        /// </summary>
        public Dictionary<int, Activity> Activities { get; set; }

        /// <summary>
        /// Available rooms for the timetable.
        /// </summary>
        public Dictionary<int, Room> Rooms { get; set; }

        /// <summary>
        /// Constraints on time resources for this timetable.
        /// </summary>
        public Dictionary<int, AbstractConstraint> TimeConstraints { get; set; }

        /// <summary>
        /// Constraints on space / location resources for this timetable.
        /// </summary>
        public Dictionary<int, AbstractConstraint> SpaceConstraints { get; set; }

        private readonly Dictionary<Type, object> _typeDict;

        /// <summary>
        /// Try and retrieve value from collection.
        /// </summary>
        /// <typeparam name="T">Type of collection</typeparam>
        /// <param name="key">Key to retrieve value for.</param>
        /// <param name="collection">Collection to find key in.</param>
        /// <returns>Value of type T belonging to thet specified key.</returns>
        public T GetValue<T>(int? key, Dictionary<int, T> collection)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (!collection.ContainsKey(key.Value)) throw new KeyNotFoundException("Could not find specified key.");

            return collection[key.Value];
        }

    }
}
