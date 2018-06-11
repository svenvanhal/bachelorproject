﻿using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Moq;
using NUnit.Framework;
using Timetabling.DB;
using Timetabling.Objects.Constraints.TimeConstraints;

namespace Timetabling.Tests.Objects.Constraints.TimeConstraints.Tests
{
    internal class ConstraintMinDaysBetweenActivitiesTest
    {
        Mock<DataModel> test;

        [SetUp]
        public void Init()
        {
            var data = new List<tt_ActitvityGroup>{
                new tt_ActitvityGroup{ classId = 1, subjectId = 1,  teacherId = 0, gradeId = 60, ActivityRefID = 1},
                new tt_ActitvityGroup{ classId = 2, subjectId = 0,  teacherId = 4, gradeId = 60, ActivityRefID = 2},
            }.AsQueryable();

            var mockSet = new Mock<DbSet<tt_ActitvityGroup>>();
            mockSet.As<IQueryable<tt_ActitvityGroup>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<tt_ActitvityGroup>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<tt_ActitvityGroup>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<tt_ActitvityGroup>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var data2 = new List<School_Lookup_Class>{
                new School_Lookup_Class{ClassName = "test", ClassID = 1, GradeID = 60},
                new School_Lookup_Class{ClassName = "test2", ClassID = 2, GradeID = 60},
            }.AsQueryable();

            var mockSet2 = new Mock<DbSet<School_Lookup_Class>>();
            mockSet2.As<IQueryable<School_Lookup_Class>>().Setup(m => m.Provider).Returns(data2.Provider);
            mockSet2.As<IQueryable<School_Lookup_Class>>().Setup(m => m.Expression).Returns(data2.Expression);
            mockSet2.As<IQueryable<School_Lookup_Class>>().Setup(m => m.ElementType).Returns(data2.ElementType);
            mockSet2.As<IQueryable<School_Lookup_Class>>().Setup(m => m.GetEnumerator()).Returns(data2.GetEnumerator());

            var data3 = new List<Subject_SubjectGrade>{
                new Subject_SubjectGrade{GradeID = 60, NumberOfLlessonsPerWeek = 4,NumberOfLlessonsPerDay =1,  SubjectID =1
                }
            }.AsQueryable();

            var mockSet3 = new Mock<DbSet<Subject_SubjectGrade>>();
            mockSet3.As<IQueryable<Subject_SubjectGrade>>().Setup(m => m.Provider).Returns(data3.Provider);
            mockSet3.As<IQueryable<Subject_SubjectGrade>>().Setup(m => m.Expression).Returns(data3.Expression);
            mockSet3.As<IQueryable<Subject_SubjectGrade>>().Setup(m => m.ElementType).Returns(data3.ElementType);
            mockSet3.As<IQueryable<Subject_SubjectGrade>>().Setup(m => m.GetEnumerator()).Returns(data3.GetEnumerator());

            var data4 = new List<HR_MasterData_Employees>{
                new HR_MasterData_Employees{EmployeeID = 0, IsActive = true, IsTeacher =true
                },
                new HR_MasterData_Employees{EmployeeID = 4, IsActive = true, IsTeacher =true
                },
            }.AsQueryable();

            var mockSet4 = new Mock<DbSet<HR_MasterData_Employees>>();
            mockSet4.As<IQueryable<HR_MasterData_Employees>>().Setup(m => m.Provider).Returns(data4.Provider);
            mockSet4.As<IQueryable<HR_MasterData_Employees>>().Setup(m => m.Expression).Returns(data4.Expression);
            mockSet4.As<IQueryable<HR_MasterData_Employees>>().Setup(m => m.ElementType).Returns(data4.ElementType);
            mockSet4.As<IQueryable<HR_MasterData_Employees>>().Setup(m => m.GetEnumerator()).Returns(data4.GetEnumerator());

            var data5 = new List<TeacherClassSubjectGroup>{
                 new TeacherClassSubjectGroup{GroupId = 2, teacherClassSubjectId= 1
                 }
             }.AsQueryable();

            var mockSet5 = new Mock<DbSet<TeacherClassSubjectGroup>>();
            mockSet5.As<IQueryable<TeacherClassSubjectGroup>>().Setup(m => m.Provider).Returns(data5.Provider);
            mockSet5.As<IQueryable<TeacherClassSubjectGroup>>().Setup(m => m.Expression).Returns(data5.Expression);
            mockSet5.As<IQueryable<TeacherClassSubjectGroup>>().Setup(m => m.ElementType).Returns(data5.ElementType);
            mockSet5.As<IQueryable<TeacherClassSubjectGroup>>().Setup(m => m.GetEnumerator()).Returns(data5.GetEnumerator());


            var data6 = new List<Tt_ClassGroup>{
                new Tt_ClassGroup{Id = 2,groupName = "sub1"
                 }
             }.AsQueryable();

            var mockSet6 = new Mock<DbSet<Tt_ClassGroup>>();
            mockSet6.As<IQueryable<Tt_ClassGroup>>().Setup(m => m.Provider).Returns(data6.Provider);
            mockSet6.As<IQueryable<Tt_ClassGroup>>().Setup(m => m.Expression).Returns(data6.Expression);
            mockSet6.As<IQueryable<Tt_ClassGroup>>().Setup(m => m.ElementType).Returns(data6.ElementType);
            mockSet6.As<IQueryable<Tt_ClassGroup>>().Setup(m => m.GetEnumerator()).Returns(data6.GetEnumerator());

            var mockDB = new Mock<DataModel>();
            mockDB.Setup(item => item.tt_ActitvityGroup).Returns(mockSet.Object);
            mockDB.Setup(item => item.School_Lookup_Class).Returns(mockSet2.Object);
            mockDB.Setup(item => item.Subject_SubjectGrade).Returns(mockSet3.Object);
            mockDB.Setup(item => item.HR_MasterData_Employees).Returns(mockSet4.Object);
            mockDB.Setup(item => item.TeacherClassSubjectGroups).Returns(mockSet5.Object);
            mockDB.Setup(item => item.Tt_ClassGroup).Returns(mockSet6.Object);


            test = mockDB;
        }

        [Test()]
        public void TestConstruct()
        {
            var constraint = new ConstraintMinDaysBetweenActivities();
            Assert.AreEqual("ConstraintMinDaysBetweenActivities", constraint.ToXelement().Name.ToString());
        }


        [Test]
        public void CreateNumberOfActivitiesTest()
        {
            var constraint = new ConstraintMinDaysBetweenActivities();

            var result = constraint.Create(test.Object);
            Assert.AreEqual("4", result.First().Elements("Number_of_Activities").First().Value);
            Assert.AreNotEqual("6", result.First().Elements("Number_of_Activities").First().Value);
        }

        [Test]
        public void CreateNumberOfActivityElementsTest()
        {
            var constraint = new ConstraintMinDaysBetweenActivities();
            var result = constraint.Create(test.Object);

            Assert.AreEqual(4, result.First().Elements("Activity_Id").Count());
        }
    }

}
