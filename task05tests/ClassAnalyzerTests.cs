using Xunit;
using task05;

public class TestClass
{
    public int PublicField;
    private string _privateField = string.Empty;
    public int Property { get; set; }

    public bool MethodWithParametersAndReturn(int parameter1, ref string parameter2, bool parameter3)
        => true;

    public void MethodWithOutParametersAndReturn() { }
}

[Serializable]
public class AttributedClass { }

public class ClassAnalyzerTests
{
    [Fact]
    public void GetPublicMethods_ReturnsCorrectMethods()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        var methods = analyzer.GetPublicMethods();

        Assert.Contains("MethodWithParametersAndReturn", methods);
        Assert.Contains("MethodWithOutParametersAndReturn", methods);
    }

    [Fact]
    public void GetMethodParams_ReturnsCorrectParameters()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        var parameters = analyzer.GetMethodParams("MethodWithParametersAndReturn");

        Assert.Contains("Int32 parameter1", parameters);
        Assert.Contains("ref String& parameter2", parameters);
        Assert.Contains("Boolean parameter3", parameters);
        Assert.Contains("Boolean", parameters);
    }

    [Fact]
    public void GetMethodParams_ReturnsReturnOfEmptyMethod()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        var parameters = analyzer.GetMethodParams("MethodWithOutParametersAndReturn");

        Assert.Contains("Void", parameters);
    }

    [Fact]
    public void GetAllFields_IncludesPrivateFields()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        var fields = analyzer.GetAllFields();

        Assert.Contains("_privateField", fields);
    }

    [Fact]
    public void GetProperties_ReturnsCorrectProperties()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        var properties = analyzer.GetProperties();

        Assert.Contains("Property", properties);
    }

    [Fact]
    public void HasAttribute_ReturnsFalse()
    {
        var analyzer = new ClassAnalyzer(typeof(AttributedClass));
        var result = analyzer.HasAttribute<SerializableAttribute>();

        Assert.True(result);
    }
}
