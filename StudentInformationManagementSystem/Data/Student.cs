namespace StudentInformationManagementSystem.Data
{
    public partial class Student
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Gender { get; set; }

        public string Images { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string Email { get; set; } = null!;

        public string Address { get; set; }


        public virtual ICollection<Enrollment> Enrollments { get; } = new List<Enrollment>();
    }
}
