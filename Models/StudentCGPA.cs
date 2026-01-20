using System.ComponentModel.DataAnnotations.Schema;

namespace StudentGradeApi.Models
{

    [Table("student_cgpa_view")]
    public class StudentCgpa
    {
        [Column("student_id")]
        public string StudentId { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("calculated_cgpa")]
        public decimal CalculatedCgpa { get; set; }
    }
}
