using System.Text.Json;
using System.Text.Json.Serialization;

namespace task13;

public class Subject
{
    public string Name { get; set; }
    public int Grade { get; set; }

    public Subject(string name, int grade)
    {
        Name = name;
        Grade = grade;
    }
}

public class Student
{

    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    [JsonConverter(typeof(DateFormatConverter))]
    public DateTime? BirthDate { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<Subject>? Grades { get; set; }

    public Student(string? firstName, string? lastName, DateTime? birthDate, List<Subject>? grades)
    {
        FirstName = firstName;
        LastName = lastName;
        BirthDate = birthDate;
        Grades = grades;
    }
}

public class Serializator
{
    public static void Serialize(Student student, string path)
    {
        if (student is null) throw new Exception("Student cannot be null");

        string json = JsonSerializer.Serialize(student, new JsonSerializerOptions
        {
            WriteIndented = true,
            IncludeFields = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        });

        File.WriteAllText(path, json);
    }

    public static Student Deserialize(string path)
    {
        string json = File.ReadAllText(path);

        if (string.IsNullOrWhiteSpace(json)) throw new Exception("File at this path is empty");

        Student student = JsonSerializer.Deserialize<Student>(json)!;

        if (student is null) throw new Exception("Object student at this path is null");

        return student;
    }
}

public class DateFormatConverter : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => DateTime.Parse(reader.GetString()!);

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        => writer.WriteStringValue(value.ToString("yyyy.MM.dd"));
}
