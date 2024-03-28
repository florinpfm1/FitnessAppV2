namespace FitnessApp2.Models.DbEntities
{
    public class CourseGuest
    {
        public int CourseId { get; set; }
        public int GuestId { get; set; }
        public Course Course { get; set; }
        public Guest Guest { get; set; }
    }
}
