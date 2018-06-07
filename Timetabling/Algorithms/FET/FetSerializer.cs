using System.Xml.Linq;
using Timetabling.Resources;

namespace Timetabling.Algorithms.FET
{

    static class FetSerializer
    {

        internal static XElement Serialize(Day element)        => new XElement("Day", new XElement("Name", element.Name));
        internal static XElement Serialize(Timeslot element)   => new XElement("Hour", new XElement("Name", element.Name));
        internal static XElement Serialize(Subject element)    => new XElement("Subject", new XElement("Name", element.Id));
        internal static XElement Serialize(Teacher element)    => new XElement("Teacher", new XElement("Name", element.Id));
        internal static XElement Serialize(Room element)       => new XElement("Room", new XElement("Name", element.Id));
        internal static XElement Serialize(SubGroup element)   => new XElement("Subgroup", new XElement("Name", element.Id));

        internal static XElement Serialize(StudentSet element)
        {
            var studentSet = new XElement("Year", new XElement("Name", element.Id));
            foreach (var group in element.Groups) { studentSet.Add(Serialize(group.Value)); }
            return studentSet;
        }

        internal static XElement Serialize(Group element)
        {
            var group = new XElement("Year", new XElement("Group", element.Id));
            foreach (var subgroup in element.SubGroups) { group.Add(Serialize(subgroup.Value)); }
            return group;
        }

        internal static XElement Serialize(Activity element)
        {
            var group = new XElement("Activity",
                new XElement("Id", element.Id),
                new XElement("Activity_Group_Id", element.GroupId),
                new XElement("Teacher", element.Teacher.Name),
                new XElement("Subject", element.Subject.Name),
                new XElement("Students", element.Students.Name),
                new XElement("Duration", element.Duration),
                new XElement("Total_Duration", element.TotalDuration),
                new XElement("Active", true));
            return group;
        }

    }
}
