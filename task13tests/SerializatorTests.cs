using Xunit;
using task13;

namespace task13tests;

public class StudentTests
{
    [Fact]
    public void Serializator_ShouldSerializeAndDeserializeCorrectly()
    {
        Student student = new Student("Artur", "Temerbalinov", new DateTime(2006, 6, 29), new List<Subject>
        {
            new Subject("Programming", 4),
            new Subject("Math", 5)
        });

        string testFile = "testFile.json";

        Serializator.Serialize(student, testFile);
        var result = Serializator.Deserialize(testFile);

        File.Delete(testFile);

        Assert.Equal(student.FirstName, result.FirstName);
        Assert.Equal(student.LastName, result.LastName);
        Assert.Equal(student.BirthDate, result.BirthDate);
        Assert.Equal("Programming", result.Grades![0].Name);
        Assert.Equal(4, result.Grades![0].Grade);
        Assert.Equal("Math", result.Grades![1].Name);
        Assert.Equal(5, result.Grades![1].Grade);
    }

    [Fact]
    public void Serializator_SerializeShouldIgnoreNullFields()
    {
        Student student = new Student(null, null, null, null);

        string testFile = "testFile.json";

        Serializator.Serialize(student, testFile);

        string json = File.ReadAllText(testFile);
        File.Delete(testFile);

        Assert.DoesNotContain("FirstName", json);
        Assert.DoesNotContain("LastName", json);
        Assert.DoesNotContain("BirthDate", json);
        Assert.DoesNotContain("Grades", json);
    }

    [Fact]
    public void Serializator_DeserializeShouldIgnoreNullFields()
    {
        Student student = new Student(null, "Temerbalinov", new DateTime(2006, 6, 29), null);

        string testFile = "testFile.json";

        Serializator.Serialize(student, testFile);
        Student deserializedStudent = Serializator.Deserialize(testFile);

        File.Delete(testFile);

        Assert.Null(deserializedStudent.FirstName);
        Assert.Null(deserializedStudent.Grades);
    }

    [Fact]
    public void Serializator_SerializeShouldThrowException_WhenStudentIsNull()
    {
        string testFile = "testFile.json";

        var exception = Assert.Throws<Exception>(() => Serializator.Serialize(null!, testFile));
        Assert.Equal("Student cannot be null", exception.Message);
    }

    [Fact]
    public void Serializator_DeserializeShouldThrowException_WhenJsonFileIsEmpty()
    {
        File.WriteAllText("EmptyJsonFile.json", "");

        var exception = Assert.Throws<Exception>(() => Serializator.Deserialize("EmptyJsonFile.json"));
        Assert.Equal("File at this path is empty", exception.Message);
    }

    [Fact]
    public void Serializator_DeserializeShouldThrowException_WhenStudentIsNull()
    {
        File.WriteAllText("testFile.json", "null");

        var exception = Assert.Throws<Exception>(() => Serializator.Deserialize("testFile.json"));
        Assert.Equal("Object student at this path is null", exception.Message);
    }
}
