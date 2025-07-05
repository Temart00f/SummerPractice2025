using Xunit;
using System.Reflection;
using task07;

public class AttributeReflectionTests
{
    [Fact]
    public void Class_HasDisplayNameAttribute()
    {
        var type = typeof(SampleClass);
        var attribute = type.GetCustomAttribute<DisplayNameAttribute>();
        ReflectionHelper.PrintTypeInfo(type);
        Assert.NotNull(attribute);
        Assert.Equal("Пример класса", attribute.DisplayName);
    }

    [Fact]
    public void Method_HasDisplayNameAttribute()
    {
        var method = typeof(SampleClass).GetMethod("TestMethod")!;
        var attribute = method.GetCustomAttribute<DisplayNameAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("Тестовый метод", attribute.DisplayName);
    }

    [Fact]
    public void Property_HasDisplayNameAttribute()
    {
        var prop = typeof(SampleClass).GetProperty("Number")!;
        var attribute = prop.GetCustomAttribute<DisplayNameAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("Числовое свойство", attribute.DisplayName);
    }

    [Fact]
    public void Class_HasVersionAttribute()
    {
        var type = typeof(SampleClass);
        var attribute = type.GetCustomAttribute<VersionAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal(1, attribute.Major);
        Assert.Equal(0, attribute.Minor);
    }

    [Fact]
    public void PrintTypeInfo_OutputCorrectLines()
    {
        var originalOutput = Console.Out;
        var stringWriter = new StringWriter();
        Console.SetOut(stringWriter);

        var type = typeof(SampleClass);
        ReflectionHelper.PrintTypeInfo(type);

        Console.SetOut(originalOutput);
        var actualOutput = stringWriter.ToString();

        Assert.Contains("Пример класса", actualOutput);
        Assert.Contains("1.0", actualOutput);
        Assert.Contains("Number / Числовое свойство", actualOutput);
        Assert.Contains("TestMethod / Тестовый метод", actualOutput);
    }
}
