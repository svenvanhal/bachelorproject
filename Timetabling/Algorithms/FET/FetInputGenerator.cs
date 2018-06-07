using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Xml.Linq;
using Timetabling.Resources;

namespace Timetabling.Algorithms.FET
{

    /// <summary>
    /// Class responsibility:
    ///  - Take generic objects
    ///  - Convert to XML
    ///  - Write XML to disk and return path
    /// </summary>
    public class FetInputGenerator
    {

        private readonly TimetableResourceCollection _res;
        private readonly IFileSystem _fs;

        /// <inheritdoc />
        public FetInputGenerator(TimetableResourceCollection resources) : this(resources, new FileSystem()) { }

        /// <summary>
        /// Create new FetInputGenerator.
        /// </summary>
        /// <param name="fileSystem">Filesystem to use.</param>
        internal FetInputGenerator(TimetableResourceCollection resources, IFileSystem fileSystem)
        {

            _fs = fileSystem;

            ToFet(resources ?? CreateTestObject());

            Console.Read();
        }

        /// <summary>
        /// Serialize Timetabling resources to FET-compatible XML file.
        /// </summary>
        /// <param name="resources">Timetabling resources.</param>
        protected void ToFet(TimetableResourceCollection resources)
        {
            if (resources == null) throw new ArgumentNullException(nameof(resources));

            // Create XDocument
            var root = new XElement("fet", new XAttribute("version", "5.35.7"));
            var document = new XDocument();
            document.Add(root);

            // Build FET XML structure
            root.Add(SerializeDays(resources.Days));
            root.Add(SerializeTimeslots(resources.Timeslots));
            root.Add(Serialize("Subject_List", resources.Subjects));
            root.Add(Serialize("Teachers_List", resources.Teachers));
            root.Add(Serialize("Students_List", resources.Students));
            root.Add(Serialize("Activities_List", resources.Activities));
            root.Add(Serialize("Rooms_List", resources.Rooms));

            Console.Write(document.ToString());

            Console.Read();

        }

        /// <summary>
        /// Serializes a subclass of Element to XML.
        /// N.B.: The element type is only checked on runtime, so FetSerializer.Serialize() MUST have an overload for all subclasses of Element to prevent any unexpected exceptions.
        /// </summary>
        /// <typeparam name="T">Type of the input resource element</typeparam>
        /// <param name="elementName">Name of the containing element</param>
        /// <param name="resources">Timetabling resources</param>
        /// <returns>An XElement.</returns>
        protected XElement Serialize<T>(string elementName, Dictionary<int, T> resources) where T : Element
        {
            if (resources == null) throw new ArgumentNullException(nameof(resources));
            var container = new XElement(elementName);

            // Add elements
            foreach (var element in resources) { container.Add(FetSerializer.Serialize((dynamic)element.Value)); }

            return container;
        }

        protected XElement SerializeDays(Dictionary<int, Day> resources)
        {
            var element = Serialize("Days_List", resources);

            // Add number of days
            element.AddFirst(new XElement("Number_of_Days", resources.Count));
            return element;
        }

        protected XElement SerializeTimeslots(Dictionary<int, Timeslot> resources)
        {
            var element = Serialize("Hours_List", resources);

            // Add number of hours
            element.AddFirst(new XElement("Number_of_Days", resources.Count));
            return element;
        }


        private TimetableResourceCollection CreateTestObject()
        {

            var resources = new TimetableResourceCollection
            {


                Days = new Dictionary<int, Day>()
                {
                    {1, new Day {Name = "Monday"}}
                },

                Timeslots = new Dictionary<int, Timeslot>()
                {
                    {1, new Timeslot {Name = "08:00 - 09:00"}}
                },

                Rooms = new Dictionary<int, Room>
                {
                    {1, new Room {Id = 1, Name = "Lab"}}
                },

                Subjects = new Dictionary<int, Subject>
                {
                    {2, new Subject {Id = 2, Name = "Computer Science"}}
                },

                Teachers = new Dictionary<int, Teacher>
                {
                    {59, new Teacher {Id = 59, Name = "Sven"}}
                },

                Students = new Dictionary<int, StudentSet>
                {
                    {
                        1, new StudentSet
                        {
                            Id = 1,
                            Name = "2018",
                            Groups = new Dictionary<int, Group>
                            {
                                { 1, new Group
                                    {
                                        Id = 1,
                                        Name = "GroupName",
                                        SubGroups = new Dictionary<int, SubGroup>
                                        {
                                            {1, new SubGroup {Id = 1, Name = "SubgroupName"} },
                                            {2, new SubGroup {Id = 2, Name = "SubgroupName2"} }
                                        }
                                    }
                                }
                            }
                        }
                    }
                },
            };

            resources.Activities = new Dictionary<int, Activity> {
                {
                    1, new Activity()
                    {
                        Id = 1,
                        GroupId = 2,
                        Teacher = resources.GetValue(59, resources.Teachers),
                        Subject = resources.GetValue(2, resources.Subjects),
                        Students = resources.GetValue(1, resources.Students),
                        Duration = 1,
                        Lessons = 10
                    }
                }
            };

            return resources;
        }
    }
}
