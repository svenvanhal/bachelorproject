﻿using System.Linq;
using System.Xml.Linq;
using Timetabling.DB;

namespace Timetabling.Objects
{
    
    /// <summary>
    /// Teachers list.
    /// </summary>
    public class TeachersList : AbstractList
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Timetabling.Objects.TeachersList"/> class.
        /// </summary>
        /// <param name="_dB">Database model.</param>
        public TeachersList(DataModel _dB) : base(_dB) => SetListElement("Teachers_List");

        /// <summary>
        /// Creates the Teacher elements from the datamodel
        /// </summary>
        public override XElement Create()
        {
            var query = dB.Employees.Where(teacher => teacher.IsTeacher == true && teacher.IsActive == true)
                          .Select(teacher => teacher.EmployeeId);

            foreach (var item in query)
            {
                List.Add(new XElement("Teacher", new XElement("Name", item)));
            }

            return List;
        }

    }
}
