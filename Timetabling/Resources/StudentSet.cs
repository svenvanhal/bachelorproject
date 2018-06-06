using System.Collections.Generic;

namespace Timetabling.Resources
{

    public class StudentSet
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public int StudentCount { get; set; }
        public Dictionary<int, Group> Groups { get; set; } = new Dictionary<int, Group>();

    }
}
