using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Timetabling.Resources
{


    public class Group
    {

        public string Name { get; set; }
        public int StudentCount { get; set; }
        public List<SubGroup> SubGroups { get; set; }

    }
}
