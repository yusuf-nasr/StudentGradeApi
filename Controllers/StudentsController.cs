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

    [HttpGet("rank/all")]
    public async Task<ActionResult<IEnumerable<StudentCgpaDto>>> GetStudentsWithCgpa()
    {
        return Ok(GetRankedStudents(await _context.StudentCgpas.ToListAsync()));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<StudentDto>> GetStudentDetails(string id)
    {
        var student = await _context.Students
            .FirstOrDefaultAsync(s => s.StudentId == id);

        if (student == null) return NotFound();

        var CGPA = await CalculateCGPA(student);
        var Rank = await CalculateStudentRankAsync(CGPA);
        return Ok(MapStudentToDto(student, CGPA, Rank));
    }

    [HttpGet("rank/{name}")]
    public async Task<ActionResult<IEnumerable<RankDto>>> GetRankBySearch(string name)
    {
        var allStudents = await _context.StudentCgpas
            .OrderByDescending(s => s.CalculatedCgpa)
            .ToListAsync();

        var rankedList = GetRankedStudents(allStudents);

        var filteredResults = rankedList
            .Where(s => s.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
            .OrderBy(s => s.Rank)
            .ToList();

        return Ok(filteredResults);
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
            Year = e.Course.YearNum
        };
    
    private List<StudentRankDto> GetRankedStudents(List<StudentCgpa> students)
    {
        var sorted = students.OrderByDescending(s => s.CalculatedCgpa).ToList();
        var result = new List<StudentRankDto>();

        int currentRank = 1;

        for (int i = 0; i < sorted.Count; i++)
        {
            // If it's not the first student and CGPA changed, 
            // the new rank is the current index + 1
            if (i > 0 && sorted[i].CalculatedCgpa < sorted[i - 1].CalculatedCgpa)
            {
                currentRank = i + 1;
            }

            result.Add(new StudentRankDto 
            { 
                Name = sorted[i].Name, 
                CGPA = sorted[i].CalculatedCgpa, 
                Rank = currentRank 
            });
        }

    return result;
    }
    private async Task<int> CalculateStudentRankAsync(decimal studentCgpa)
    {
        int countBetterStudents = await _context.StudentCgpas
            .CountAsync(s => Math.Round(s.CalculatedCgpa, 2) > Math.Round(studentCgpa, 2));

        return countBetterStudents + 1;
    }
    private async Task<decimal> CalculateCGPA(Student s)
    {
        decimal x = 0, y = 0;
        foreach(var e in s.Enrollments)
        {
            x += (e.Grade * e.Course.Credits);
            y += e.Course.Credits;
        }
        return Math.Round(y == 0 ? 0 : (x / y), 4);
    }
}