using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentGradeApi.Models
{
    [Table("courses")]
    public class Course
    {
        [Key]
        [Column("course_name")]
        public string CourseName { get; set; }

        [Column("credits")]
        public int Credits { get; set; }

        [Column("year_num")]
        public int YearNum { get; set; }
    }
}

