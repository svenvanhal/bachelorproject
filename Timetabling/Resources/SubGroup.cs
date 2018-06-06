using System.Runtime.Serialization;

namespace Timetabling.Resources
{

    public class SubGroup
    {

        public string Name { get; set; }
        public int StudentCount { get; set; }

        public SubGroup() { }
        public SubGroup(string name, int studentCount)
        {
            Name = name;
            StudentCount = studentCount;
        }

    }
}
