using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Runtime.Serialization;
using System.Xml.Linq;
using System.Xml.Serialization;
using Timetabling.Resources;
using Timetabling.Resources.Constraints;

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

            var res = CreateTestObject();
            ToFet(res);

            Console.Read();
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
                    {1, new Room {Name = "Lab"}}
                },

                Subjects = new Dictionary<int, Subject>
                {
                    {1, new Subject {Name = "Computer Science"}}
                },

                Teachers = new Dictionary<int, Teacher>
                {
                    {1, new Teacher {Name = "Sven"}}
                },

                Students = new Dictionary<int, StudentSet>
                {
                    {
                        1, new StudentSet
                        {
                            Name = "2018",
                            StudentCount = 1,
                            Groups = new Dictionary<int, Group>
                            {
                                { 1, new Group
                                    {
                                        Name = "GroupName",
                                        StudentCount = 1,
                                        SubGroups = new Dictionary<int, SubGroup>
                                        {
                                            {1, new SubGroup {Name = "SubgroupName", StudentCount = 1} },
                                            {2, new SubGroup {Name = "SubgroupName2", StudentCount = 1} }
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
                        Teacher = resources.GetValue(1, resources.Teachers),
                        Subject = resources.GetValue(1, resources.Subjects),
                        Students = resources.GetValue(1, resources.Students),
                        Duration = 1,
                        Lessons = 10
                    }
                }
            };

            return resources;
        }

        /// <summary>
        /// Serialize Timetabling resources to FET-compatible XML file.
        /// </summary>
        /// <param name="resources">Timetabling resources.</param>
        /// <param name="overrides">XML overrides.</param>
        protected void ToFet(TimetableResourceCollection resources)
        {
            if (resources == null) throw new ArgumentNullException(nameof(resources));

            // Create XDocument
            var root = new XElement("fet", new XAttribute("version", "5.35.7"));
            var document = new XDocument();
            document.Add(root);

            // Build FET XML structure
            //root.Add(Serialize("Days_List", resources.Days));
            //root.Add(Serialize("Hour_List", resources.Timeslots));

            Console.Write(document.ToString());

            Console.Read();

        }

        protected XElement SerializeElement<Element>(Day element)
        {
            return new XElement("Day", new XElement("Name", element.Name));
        }

        protected XElement SerializeElement<Element>(Timeslot element)
        {
            return new XElement("Day", new XElement("Name", element.Name));
        }

        protected XElement SerializeDays(TimetableResourceCollection resources)
        {
            // Validation
            if(resources == null) throw new ArgumentNullException(nameof(resources));
            if(resources.Days == null) throw new ArgumentNullException(nameof(resources), $"The Days object in { nameof(resources) } is null.");

            // Create Days wrapper
            var container = new XElement("Days_List");

            foreach (var day in resources.Days) {
                container.Add(new XElement("Day", new XElement("Name", day.Value.Name)));
            }

            return container;
        }

        protected XElement SerializeTimeslots(TimetableResourceCollection resources)
        {
            // Validation
            if (resources == null) throw new ArgumentNullException(nameof(resources));
            if (resources.Timeslots == null) throw new ArgumentNullException(nameof(resources), $"The Timeslots object in { nameof(resources) } is null.");

            // Create Days wrapper
            var container = new XElement("Hours_List");

            foreach (var hour in resources.Timeslots) {
                container.Add(new XElement("Hour", new XElement("Name", hour.Value.Name)));
            }

            return container;
        }

        protected XElement SerializeSubjects(TimetableResourceCollection resources)
        {
            // Validation
            if (resources == null) throw new ArgumentNullException(nameof(resources));
            if (resources.Subjects == null) throw new ArgumentNullException(nameof(resources), $"The Subjects object in { nameof(resources) } is null.");

            // Create Days wrapper
            var container = new XElement("Subjects_List");

            foreach (var subject in resources.Subjects)
            {
                container.Add(new XElement("Subject",
                    new XElement("Name", subject.Value.Name)));
            }

            return container;
        }

    }
}
