using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentGradeApi.Models
{

    [Table("students")]
    public class Student
    {
        [Key]
        [Column("student_id")]
        public string StudentId { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("email")]
        public string Email { get; set; }

        // Navigation Property
        public virtual ICollection<Enrollment> Enrollments { get; set; }
    }
}
