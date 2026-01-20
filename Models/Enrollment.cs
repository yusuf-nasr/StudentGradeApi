using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentGradeApi.Models
{
    [Table("enrollments")]
    public class Enrollment
    {
        [Key]
        [Column("enrollment_id")]
        public int EnrollmentId { get; set; }

        [Column("student_id")]
        [ForeignKey("Student")]
        public string StudentId { get; set; }

        [Column("course_name")]
        [ForeignKey("Course")]
        public string CourseName { get; set; }

        [Column("grade")]
        public decimal Grade { get; set; }

        [Column("term_type")]
        public string TermType { get; set; }

        public Student Student { get; set; }
        public Course Course { get; set; }
    }
}

