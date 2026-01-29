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
        var list = await _context.StudentCgpas.OrderByDescending(s => s.CalculatedCgpa)
                 .Select(s => new StudentCgpaDto
                {
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

        var CGPA = CalculateCGPA(student);
        var Rank = await CalculateStudentRankAsync(CGPA);
        return Ok(MapStudentToDto(student, CGPA, Rank));
    }
    
    [HttpGet("rank/search/{name}")]
    public async Task<ActionResult<IEnumerable<StudentDto>>> GetRankBySearch(string name)
    {
        var searchResults = await _context.StudentCgpas
        .Where(s => s.Name.Contains(name))
        .ToListAsync();

        var resultList = new List<RankDto>();

        foreach (var student in searchResults)
        {
            int rank = await CalculateStudentRankAsync(student.CalculatedCgpa);
            var dto = new RankDto
            {
                Rank = rank,
                Name = student.Name,
                CGPA = student.CalculatedCgpa
            };

            resultList.Add(dto);
        }

        return Ok(resultList.OrderBy(s => s.Rank));
    }

    private static StudentDto MapStudentToDto(Student s, decimal cgpa, int rank) =>
        new StudentDto
        {
            StudentId = s.StudentId,
            Name = s.Name,
            Email = s.Email,
            Cgpa = cgpa,
            Rank = rank,
            Enrollments = s.Enrollments?.Select(MapEnrollmentToDto).ToList() ?? new List<EnrollmentDto>()
        };

    private static EnrollmentDto MapEnrollmentToDto(Enrollment e) =>
        new EnrollmentDto
        {
            CourseName = e.CourseName,
            Grade = e.Grade,
        };
    private async Task<int> CalculateStudentRankAsync(decimal studentCgpa)
    {
        int countBetterStudents = await _context.StudentCgpas
            .CountAsync(s => s.CalculatedCgpa > studentCgpa);

        return countBetterStudents + 1;
    }
    private decimal CalculateCGPA(Student s)
    {
        decimal x = 0, y = 0;
        foreach(var e in s.Enrollments)
        {
            x += (e.Grade * e.Course.Credits);
            y += e.Course.Credits;
        }
        return (y == 0 ? 0 : (x / y));
    }
}