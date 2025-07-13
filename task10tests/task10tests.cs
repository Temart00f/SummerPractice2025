using task10;
using Xunit;
using System.Reflection;

namespace task10tests;

public class PluginLoaderTests
{
    [Fact]
    public void PluginLoader_ShouldExecutePluginsInCorrectOrder()
    {
        var loader = new PluginLoader();

        var currentDirectory = new DirectoryInfo(Directory.GetCurrentDirectory());
        var parentDirectory = currentDirectory.Parent!.Parent!.Parent!.Parent!.FullName;
        var path = Path.Combine(parentDirectory, "Plugins");

        var originalOutput = Console.Out;
        var stringWtiter = new StringWriter();
        Console.SetOut(stringWtiter);

        loader.LoadPlugins(path);

        Console.SetOut(originalOutput);
        var actualOutput = stringWtiter.ToString();
        var expectedOutput = "Plugin4 was executed\nPlugin3 was executed\nPlugin2 was executed\nPlugin1 was executed";

        Assert.Contains(expectedOutput, actualOutput);
    }

    [Fact]
    public void PluginLoader_ShouldThrowException()
    {
        var loader = new PluginLoader();
        var path = "UncorrectPath";
        var exception = Assert.Throws<DirectoryNotFoundException>(() => loader.LoadPlugins(path));

        Assert.Equal("Path is uncorrect", exception.Message);
    }
}