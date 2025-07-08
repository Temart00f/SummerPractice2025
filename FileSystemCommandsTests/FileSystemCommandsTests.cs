using FileSystemCommands;

public class FileSystemCommandsTests
{
    [Fact]
    public void DirectorySizeCommand_ShouldCalculateSize()
    {
        var testDir = Path.Combine(Path.GetTempPath(), "TestDir");
        var testSubDir = Path.Combine(testDir, "TestSubDir");
        Directory.CreateDirectory(testDir);
        Directory.CreateDirectory(testSubDir);
        File.WriteAllText(Path.Combine(testDir, "test1.txt"), "Hello");
        File.WriteAllText(Path.Combine(testDir, "test2.txt"), "World");
        File.WriteAllText(Path.Combine(testSubDir, "test3.txt"), "Hello");
        File.WriteAllText(Path.Combine(testSubDir, "test4.txt"), "World");

        var command = new DirectorySizeCommand(testDir);
        command.Execute();

        Directory.Delete(testDir, true);

        Assert.Equal(20, command.Size); ;
    }

    [Fact]
    public void FindFilesCommand_ShouldFindMatchingFiles()
    {
        var testDir = Path.Combine(Path.GetTempPath(), "TestDir");
        var testSubDir = Path.Combine(testDir, "TestSubDir");
        Directory.CreateDirectory(testDir);
        Directory.CreateDirectory(testSubDir);
        File.WriteAllText(Path.Combine(testDir, "file1.txt"), "Text");
        File.WriteAllText(Path.Combine(testDir, "file2.log"), "Log");
        File.WriteAllText(Path.Combine(testSubDir, "file3.txt"), "Text");
        File.WriteAllText(Path.Combine(testSubDir, "file4.log"), "Log");

        var commandTxt = new FindFilesCommand(testDir, "*.txt");
        var commandLog = new FindFilesCommand(testDir, "*.log");
        commandTxt.Execute();
        commandLog.Execute();

        Directory.Delete(testDir, true);

        Assert.Equal(2, commandTxt.FoundedFiles.Count);
        Assert.Equal(2, commandLog.FoundedFiles.Count);
        Assert.Contains(Path.Combine(testDir, "file1.txt"), commandTxt.FoundedFiles);
        Assert.Contains(Path.Combine(testSubDir, "file3.txt"), commandTxt.FoundedFiles);
        Assert.Contains(Path.Combine(testDir, "file2.log"), commandLog.FoundedFiles);
        Assert.Contains(Path.Combine(testSubDir, "file4.log"), commandLog.FoundedFiles);
    }

    [Fact]
    public void DirectorySizeCommand_ShouldThrowExceptionForUnCorrectPath()
    {
        var testDir = Path.Combine(Path.GetTempPath(), "TestDir");
        var command = new DirectorySizeCommand(testDir);
        var exception = Assert.Throws<DirectoryNotFoundException>(() => command.Execute());

        Assert.Equal($"Could not find directory at path: {testDir}", exception.Message);
    }

    [Fact]
    public void FindFilesCommand_ShouldThrowExceptionForUnCorrectPath()
    {
        var testDir = Path.Combine(Path.GetTempPath(), "TestDir");
        var command = new FindFilesCommand(testDir, "*.txt");
        var exception = Assert.Throws<DirectoryNotFoundException>(() => command.Execute());

        Assert.Equal($"Could not find directory at path: {testDir}", exception.Message);
    }

    [Fact]
    public void CommandRunner_ShouldPrintCorrectLines()
    {
        var testDirectoryPath = Path.Combine(Path.GetTempPath(), "TestDirectory");
        var testSubDirectoryPath = Path.Combine(testDirectoryPath, "TestSubDirectory");

        var originalOutput = Console.Out;
        var stringWriter = new StringWriter();
        Console.SetOut(stringWriter);

        CommandRunner.CommandRunner.Main();

        Console.SetOut(originalOutput);
        var actualOutput = stringWriter.ToString();

        Assert.Contains("67 byte", actualOutput);
        Assert.Contains(Path.Combine(testDirectoryPath, "file1.txt"), actualOutput);
        Assert.Contains(Path.Combine(testDirectoryPath, "file2.txt"), actualOutput);
        Assert.Contains(Path.Combine(testSubDirectoryPath, "file4.txt"), actualOutput);
        Assert.Contains(Path.Combine(testSubDirectoryPath, "file5.txt"), actualOutput);
        Assert.Contains(Path.Combine(testDirectoryPath, "file3.log"), actualOutput);
        Assert.Contains(Path.Combine(testSubDirectoryPath, "file6.log"), actualOutput);
    }  
}
