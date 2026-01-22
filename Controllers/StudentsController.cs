using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentGradeApi.Data;
using StudentGradeApi.DTOs;
using StudentGradeApi.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

[Route("api/[controller]")]
[ApiController]
public class StudentsController : ControllerBase
{
    private readonly AppDbContext _context;

    public StudentsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("rank")]
    public async Task<ActionResult<IEnumerable<StudentCgpaDto>>> GetStudentsWithCgpa()
    {
        var list = await _context.StudentCgpas
                                 .OrderByDescending(s => s.CalculatedCgpa)
                                 .Select(s => new StudentCgpaDto
                                 {
                                     StudentId = s.StudentId,
                                     Name = s.Name,
                                     CalculatedCgpa = s.CalculatedCgpa
                                 })
                                 .ToListAsync();

        return Ok(list);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<StudentDto>> GetStudentDetails(string id)
    {
        var student = await _context.Students
            .Include(s => s.Enrollments)
            .FirstOrDefaultAsync(s => s.StudentId == id);

        if (student == null) return NotFound();
        var studentCgpa = await _context.StudentCgpas
            .FirstOrDefaultAsync(s => s.StudentId == id);

        decimal cgpa = studentCgpa?.CalculatedCgpa ?? 0 ;

        return Ok(MapStudentToDto(student,cgpa));
    }
    
    [HttpGet("rank/search/{name}")]
    public async Task<ActionResult<IEnumerable<StudentDto>>> GetRankBySearch(string name)
    {
        var allstudents = await _context.StudentCgpas
            .OrderByDescending(s => s.CalculatedCgpa).ToListAsync();
        var studentRanks = allstudents.Select((Student, Index) => new StudentRankDto
        {
            Rank = Index + 1,
            Name = Student.Name,
            CGPA = Student.CalculatedCgpa
        }).Where(s=>s.Name.ToLower().Contains(name.ToLower())).ToList();
        if (!studentRanks.Any()) return NotFound("no students with this name");
        return Ok(studentRanks);

    }

    private static StudentDto MapStudentToDto(Student s, decimal cgpa) =>
        new StudentDto
        {
            StudentId = s.StudentId,
            Name = s.Name,
            Email = s.Email,
            Cgpa = cgpa,
            Enrollments = s.Enrollments?.Select(MapEnrollmentToDto).ToList() ?? new List<EnrollmentDto>()
        };

    private static EnrollmentDto MapEnrollmentToDto(Enrollment e) =>
        new EnrollmentDto
        {
            EnrollmentId = e.EnrollmentId,
            CourseName = e.CourseName,
            Grade = e.Grade,
        };
}