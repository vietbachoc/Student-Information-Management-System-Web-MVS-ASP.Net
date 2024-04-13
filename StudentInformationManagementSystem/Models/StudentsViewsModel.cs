using StudentInformationManagementSystem.Data;
using System.ComponentModel.DataAnnotations;

namespace StudentInformationManagementSystem.Models
{
    public class StudentsViewsModel
    {
        [Key]

        public string firstName { get; set; }

        public string lastName { get; set; } = null!;

        public string gender { get; set; }

        public DateTime? DoB { get; set; }

        public string email { get; set; }
        public string address { get; set; }
        public IFormFile photo { get; set; }
        public int Id { get; set; }
    }
}
