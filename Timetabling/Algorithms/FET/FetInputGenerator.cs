using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Runtime.Serialization;
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
                        Teacher = _res.GetValue(1, _res.Teachers),
                        Subject = _res.GetValue(1, _res.Subjects),
                        Students = _res.GetValue(1, _res.Students),
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
        protected void ToFet(TimetableResourceCollection resources, XmlAttributeOverrides overrides = null)
        {
            if (resources == null) throw new ArgumentNullException(nameof(resources));

            // Get overrides

            var serializer = new DataContractSerializer(typeof(TimetableResourceCollection), "fet", "test", new List<Type>
            {
                typeof(BasicTimeConstraint),
                typeof(BasicSpaceConstraint),
            });

            // Read and deserialize XML
            _fs.Directory.CreateDirectory("test");
            using (var writer = _fs.File.OpenWrite("test/input.fet"))
            {
                serializer.WriteObject(writer, resources);
            }

        }

    }
}
