using System;
using Timetabling.DB;
using Timetabling.Objects;
using Timetabling.Resources;

namespace Implementation
{
    class ResourceGatherer
    {

        // Please change my name
        public ResourceGatherer(DataModel dataModel)
        {

            var daysList = new DaysList(dataModel);
            var hoursList = new HoursList(dataModel);
            var teachersList = new TeachersList(dataModel);
            var subjectsList = new SubjectsList(dataModel);

            //var activitiesList = new ActivitiesList(dataModel);
            //var yearsList = new YearsList(dataModel);
            //var timeConstraintsList = new TimeConstraintsList(dataModel);
            //var spaceConstraintsList = new SpaceConstraintsList(dataModel);
            var roomsList = new RoomsList(dataModel);

            var resources = new TimetableResourceCollection();
            resources.Days = daysList.GetDays();
            resources.Timeslots = hoursList.GetTimeslots();
            resources.Teachers = teachersList.GetTeachers();
            resources.Subjects = subjectsList.GetSubjects();
            resources.Rooms = roomsList.GetRooms();

            Console.Read();

        }

    }
}
