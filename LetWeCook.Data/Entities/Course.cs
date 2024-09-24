namespace LetWeCook.Data.Entities
{
	public class Course
	{
		public Guid Id { get; set; }
		public string Title { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public decimal Price { get; set; }
		public ApplicationUser Instructor { get; set; } = null!;
		public bool IsPublished { get; set; }
		public bool Certificate { get; set; }
		public DateTime DateCreated { get; set; }
		public List<ApplicationUser> EnrolledStudents { get; set; } = new List<ApplicationUser>();
		public List<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
		public List<Lesson> Lessons { get; set; } = new List<Lesson>();
	}
}
