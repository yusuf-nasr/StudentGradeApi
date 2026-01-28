
using System.Collections.Generic;

namespace StudentGradeApi.DTOs
{
    public class StudentDto
    {
        public string StudentId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public decimal Cgpa { get; set; }
        public int Rank { get; set; }
        public ICollection<EnrollmentDto> Enrollments { get; set; } = new List<EnrollmentDto>();
    }
}