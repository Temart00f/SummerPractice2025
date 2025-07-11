using MetadataMiner;

namespace task09tests;

public class UnitTest1
{
    [Fact]
    public void MetadataMiner_SouldPrintCorrectLines()
    {
        var currentDirectoryPath = Directory.GetCurrentDirectory();
        var currentDirectory = new DirectoryInfo(currentDirectoryPath);
        var parentDirectory = currentDirectory.Parent!.Parent!.Parent!.Parent!.FullName;
        var dllPath = Path.Combine(parentDirectory, "task07", "bin", "Debug", "net8.0", "task07.dll");

        var originalOutput = Console.Out;
        var stringWriter = new StringWriter();
        Console.SetOut(stringWriter);

        MetadataMiner.MetadataMinerClass.Main(new[] {dllPath});

        Console.SetOut(originalOutput);
        var actualOutput = stringWriter.ToString();

        Assert.Contains("DisplayNameAttribute", actualOutput);
        Assert.Contains("-.ctor(String displayName)", actualOutput);

        Assert.Contains("VersionAttribute", actualOutput);
        Assert.Contains("-.ctor(String version)", actualOutput);

        Assert.Contains("SampleClass", actualOutput);
        Assert.Contains("TestMethod()", actualOutput);
        Assert.Contains("-DisplayNameAttribute", actualOutput);
        Assert.Contains("-VersionAttribute", actualOutput);

        Assert.Contains("ClassWithOutAttributes", actualOutput);
        Assert.Contains("-EmptyMethod()", actualOutput);

        Assert.Contains("EmptyClass", actualOutput);

        Assert.Contains("ReflectionHelper", actualOutput);
        Assert.Contains("-PrintTypeInfo(Type type)", actualOutput);
    }

    [Fact]
    public void MetadataMiner_ShouldThrowException_WhenPathIsUncorrect()
    {
        Assert.Throws<FileNotFoundException>(()
        => MetadataMiner.MetadataMinerClass.Main(["UncorrectPath"]));
    }

    [Fact]
    public void MetadataMiner_ShouldThrowException_WhenPathIsEmpty()
    {
        Assert.Throws<FileNotFoundException>(()
        => MetadataMiner.MetadataMinerClass.Main([]));
    }
}
