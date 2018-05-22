﻿

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Timetabling.DB;

namespace Timetabling.Objects
{
	public class TeachersList : AbstractList
	{

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Timetabling.Objects.TeachersList"/> class.
        /// </summary>
        /// <param name="_dB">Database model.</param>
		public TeachersList(DataModel _dB) : base(_dB)
		{
			SetListElement("Teachers_List");
		}



		public override void Create()
		{
			var query = dB.HR_MasterData_Employees.Where(teacher => teacher.IsTeacher == true && teacher.IsActive == true )
			              .Select(teacher => teacher.EmployeeID);

			foreach (var item in query)
			{
				list.Add(new XElement("Teacher", new XElement("Name", item))) ;

			}
			
		}
	}
}
