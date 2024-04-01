namespace FitnessApp2.Models.DbEntities
{
    public class InstructorGuest
    {
        public int InstructorId { get; set; }
        public int GuestId { get; set; }
        public Instructor Instructor { get; set; }
        public Guest Guest { get; set; }
    }
}
