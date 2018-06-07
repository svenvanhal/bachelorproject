using System;
using System.IO;
using System.Threading.Tasks;
using Implementation.Key2Soft;
using Timetabling;
using Timetabling.Algorithms.FET;
using Timetabling.DB;
using Timetabling.Resources;

namespace Implementation
{
    class Program
    {
        static void Main(string[] args)
        {

            // Example usage:
            //   1 - Instantiate and configure algorithm to use
            //   2 - Specify input data for algorithm
            //   3 - Create a TimetableGenerator
            //   4 - Let the TimetableGenerator generate a Task<Timetable>
            //   5 - Do something with the Timetable output object when the Task finishes

            //var inputGenerator = new ResourceGatherer(new DataModel());
            //var inputGenerator = new Timetabling.Algorithms.FET.FetInputGenerator(null);

            var model = new DataModel();

            var resources = new TimetableResourceCollection
            {
                Days = DboResourceFactory.GetDays(),
                Timeslots = DboResourceFactory.GetTimeslots(),
                Teachers = DboResourceFactory.GetTeachers(model),
                Subjects = DboResourceFactory.GetSubjects(model),
                Rooms = DboResourceFactory.GetRooms(model),
                Students = DboResourceFactory.GetYears(model),
            };

            // Note: we need to pass the generated resources up to this moment!
            resources.Activities = DboResourceFactory.GetActivities(model, resources);
            resources.TimeConstraints = DboResourceFactory.GetTimeConstraints(model, resources);
            resources.SpaceConstraints = DboResourceFactory.GetSpaceConstraints(model, resources);

            return;


            var algorithm = new FetAlgorithm();

            // TODO: improve FET input file generation
            var inputFile = FetInputGenerator.GenerateFetFile(new DataModel(), Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), "fetInputGenerator")).FullName);

            var generator = new TimetableGenerator();
            var task = generator.RunAlgorithm(algorithm, inputFile);

            // On success
            task.ContinueWith(t =>
            {
                Console.WriteLine("The timetable has been generated sucessfully.");
                Console.WriteLine(task.Result);
            }, TaskContinuationOptions.NotOnFaulted);

            // On error
            task.ContinueWith(t =>
            {
                Console.WriteLine("The timetable could not be generated.");
                foreach (var ex in t.Exception.InnerExceptions) { Console.WriteLine(ex.Message); }
            }, TaskContinuationOptions.OnlyOnFaulted);

            Console.Read();
        }
    }
}
