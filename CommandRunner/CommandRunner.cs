using System.Reflection;

namespace CommandRunner;

public class CommandRunner
{
    public static void Main()
    {
        string currentDirectoryPath = Directory.GetCurrentDirectory();
        DirectoryInfo currentDirectory = new DirectoryInfo(currentDirectoryPath);
        string parentDirectory = currentDirectory.Parent!.Parent!.Parent!.Parent!.FullName;
        string dllPath = Path.Combine(parentDirectory, "FileSystemCommands\\bin\\Debug\\net8.0\\FileSystemCommands.dll");

        var testDirectory = Path.Combine(Path.GetTempPath(), "TestDirectory");
        var testSubDirectory = Path.Combine(testDirectory, "TestSubDirectory");
        Directory.CreateDirectory(testDirectory);
        Directory.CreateDirectory(testSubDirectory);
        File.WriteAllText(Path.Combine(testDirectory, "file1.txt"), "Hello from test directory");
        File.WriteAllText(Path.Combine(testDirectory, "file2.txt"), "Text");
        File.WriteAllText(Path.Combine(testDirectory, "file3.log"), "Log");
        File.WriteAllText(Path.Combine(testSubDirectory, "file4.txt"), "Hello from test subdirectory");
        File.WriteAllText(Path.Combine(testSubDirectory, "file5.txt"), "Text");
        File.WriteAllText(Path.Combine(testSubDirectory, "file6.log"), "Log");

        Assembly asm = Assembly.LoadFrom(dllPath);

        var directorySizeCommandType = asm.GetType("FileSystemCommands.DirectorySizeCommand");
        var findFilesCommandType = asm.GetType("FileSystemCommands.FindFilesCommand");

        if (directorySizeCommandType != null && findFilesCommandType != null)
        {
            object commandSize = Activator.CreateInstance(directorySizeCommandType, [testDirectory])!;
            object commandTxt = Activator.CreateInstance(findFilesCommandType, [testDirectory, "*.txt"])!;
            object commandLog = Activator.CreateInstance(findFilesCommandType, [testDirectory, "*.log"])!;

            MethodInfo executeGetSize = directorySizeCommandType.GetMethod("Execute")!;
            MethodInfo executeFindFiles = findFilesCommandType.GetMethod("Execute")!;

            executeGetSize.Invoke(commandSize, []);
            executeFindFiles.Invoke(commandTxt, []);
            executeFindFiles.Invoke(commandLog, []);

            Type commandSizeType = commandSize.GetType();
            Type commandTxtType = commandTxt.GetType();
            Type commandLogType = commandLog.GetType();

            var size = commandSizeType.GetProperty("Size")!.GetValue(commandSize);
            var foundedFilesTxt = commandTxtType.GetProperty("FoundedFiles")!.GetValue(commandTxt) as List<string>;
            var foundedFilesLog = commandLogType.GetProperty("FoundedFiles")!.GetValue(commandLog) as List<string>;

            Console.WriteLine($"Directory size: {size} byte");

            Console.WriteLine("Files obtained from test directory by mask .txt:");
            foreach (string file in foundedFilesTxt!)
            {
                Console.WriteLine(file);
            }

            Console.WriteLine("Files obtained from test directory by mask .log:");
            foreach (string file in foundedFilesLog!)
            {
                Console.WriteLine(file);
            }
            
            Directory.Delete(testDirectory, true);
        }

        else
        {
            Console.WriteLine($"Classes DirectorySizeCommand, FindFilesCommand not found");

            Directory.Delete(testDirectory, true);
        }
    }
}
