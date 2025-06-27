using Xunit;
using task02;


namespace task02tests;

public class StudentServiceTests
{
    private List<Student> _testStudents;
    private StudentService _service;

    public StudentServiceTests()
    {
        _testStudents = new List<Student>
        {
            new() { Name = "Иван", Faculty = "ФИТ", Grades = new List<int> { 5, 4, 5 } },
            new() { Name = "Анна", Faculty = "ФИТ", Grades = new List<int> { 3, 4, 3 } },
            new() { Name = "Петр", Faculty = "Экономика", Grades = new List<int> { 5, 5, 5 } },
            new() { Name = "Миша", Faculty = "МО", Grades = new List<int> { 3, 3, 3 } },
            new() { Name = "Егор", Faculty = "МО", Grades = new List<int> { 4, 5, 4 } },
            new() { Name = "Егор", Faculty = "Экономика", Grades = new List<int> { 5, 5, 5 } }
        };
        _service = new StudentService(_testStudents);
    }

    // 1.
    [Fact]
    public void GetStudentsByFaculty_ReturnsCorrectStudents()
    {
        var result = _service.GetStudentsByFaculty("ФИТ").ToList();
        Assert.Equal(2, result.Count);
        Assert.True(result.All(s => s.Faculty == "ФИТ"));
    }

    // 2.
    [Fact]
    public void GetStudentsWithMinAverageGrade_ReturnsCorrectStudents()
    {
        var result = _service.GetStudentsWithMinAverageGrade(4).ToList();
        Assert.Equal(4, result.Count);
        Assert.Equal("Иван", result[0].Name);
        Assert.Equal("Петр", result[1].Name);
        Assert.Equal("Егор", result[2].Name);
        Assert.Equal("МО", result[2].Faculty);
        Assert.Equal("Егор", result[3].Name);
        Assert.Equal("Экономика", result[3].Faculty);
    }

    // 3.
    [Fact]
    public void GetStudentsOrderedByName_ReturnsCorrectStudents()
    {
        var result = _service.GetStudentsOrderedByName().ToList();
        Assert.Equal(6, result.Count);
        Assert.Equal("Анна", result[0].Name);
        Assert.Equal("Егор", result[1].Name);
        Assert.Equal("МО", result[1].Faculty);
        Assert.Equal("Егор", result[2].Name);
        Assert.Equal("Экономика", result[2].Faculty);
        Assert.Equal("Иван", result[3].Name);
        Assert.Equal("Миша", result[4].Name);
        Assert.Equal("Петр", result[5].Name);
    }

    // 4.
    [Fact]
    public void GroupStudentsByFaculty_ReturnsCorrectGroups()
    {
        var result = _service.GroupStudentsByFaculty();
        Assert.Equal(3, result.Count);
        Assert.Equal(2, result["ФИТ"].Count());
        Assert.Equal(2, result["МО"].Count());
        Assert.Equal(2, result["Экономика"].Count());
    }

    // 5.
    [Fact]
    public void GetFacultyWithHighestAverageGrade_ReturnsCorrectFaculty()
    {
        var result = _service.GetFacultyWithHighestAverageGrade();
        Assert.Equal("Экономика", result);
    }
}
