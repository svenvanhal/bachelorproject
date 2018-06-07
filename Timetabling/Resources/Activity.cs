namespace Timetabling.Resources
{

    public class Activity : Element
    {

        public int Id { get; set; }
        public int GroupId { get; set; }
        public Teacher Teacher { get; set; }
        public Subject Subject { get; set; }
        public StudentSet Students { get; set; }
        public int Duration { get; set; }
        public int Lessons { get; set; }

        public int TotalDuration => Duration * Lessons;

    }
}
