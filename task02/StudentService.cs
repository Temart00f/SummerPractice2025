namespace task02;

public class Student
{
    public required string Name { get; set; }
    public required string Faculty { get; set; }
    public required List<int> Grades { get; set; }
}

public class StudentService
{
    private readonly List<Student> _students;

    public StudentService(List<Student> students) => _students = students;

    // 1.
    public IEnumerable<Student> GetStudentsByFaculty(string faculty)
        => _students.Where(s => s.Faculty == faculty);

    // 2. 
    public IEnumerable<Student> GetStudentsWithMinAverageGrade(double minAverageGrade)
        => _students.Where(s => s.Grades.Average() >= minAverageGrade);

    // 3.
    public IEnumerable<Student> GetStudentsOrderedByName()
        => _students.OrderBy(s => s.Name);

    // 4.
    public ILookup<string, Student> GroupStudentsByFaculty()
        => _students.ToLookup(s => s.Faculty);

    // 5.
    public string GetFacultyWithHighestAverageGrade()
     => _students
        .Where(s => s.Faculty != null)
        .GroupBy(s => s.Faculty)
        .Select(g => (Faculty: g.Key, Avg: g.Average(s => s.Grades.Average())))
        .MaxBy(x => x.Avg)
        .Faculty;
}
