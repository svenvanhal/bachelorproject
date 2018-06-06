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

            var resources = new TimetableResourceCollection
            {
                Days = DaysList.GetDays(),
                Timeslots = HoursList.GetTimeslots(),
                Teachers = TeachersList.GetTeachers(dataModel),
                Subjects = SubjectsList.GetSubjects(dataModel),
                Rooms = RoomsList.GetRooms(dataModel),
                Students = YearsList.GetYears(dataModel),
                TimeConstraints = null, // TODO
                SpaceConstraints = null // TODO
            };

            // Note: we need to pass the generated resources up to this moment!
            resources.Activities = ActivitiesList.GetActivities(dataModel, resources);

            Console.Read();

        }

    }
}
