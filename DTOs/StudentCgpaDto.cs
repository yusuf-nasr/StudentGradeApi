
namespace StudentGradeApi.DTOs
{
    public class StudentCgpaDto
    {
        public string StudentId { get; set; }
        public string Name { get; set; }
        public decimal CalculatedCgpa { get; set; }
    }
}