using System.Runtime.Serialization;

namespace Timetabling.Resources
{

    public class Timeslot
    {

        public string Name { get; set; }

        public Timeslot() { }
        public Timeslot(string name)
        {
            Name = name;
        }

    }
}
