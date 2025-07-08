using CommandLib;

namespace FileSystemCommands;

public class DirectorySizeCommand : ICommand
{
    private string Path { get; set; }
    public long Size { get; private set; }

    public DirectorySizeCommand(string path)
    {
        Path = path;
        Size = 0L;
    }

    public void Execute()
    {
        if (!Directory.Exists(Path))
        {
            throw new DirectoryNotFoundException($"Could not find directory at path: {Path}");
        }

        Queue<string> queue = new Queue<string>();
        queue.Enqueue(Path);

        while (queue.Count != 0)
        {
            string directory = queue.Dequeue();
            string[] files = Directory.GetFiles(directory);
            string[] subdirectories = Directory.GetDirectories(directory);

            foreach (string file in files)
            {
                Size += new FileInfo(file).Length;
            }

            foreach (string subdir in subdirectories)
            {
                queue.Enqueue(subdir);
            }
        }
    }
}

public class FindFilesCommand : ICommand
{
    private string Path { get; set; }
    private string Mask { get; set; }
    public List<string> FoundedFiles { get; private set; }

    public FindFilesCommand(string path, string mask)
    {
        Path = path;
        Mask = mask;
        FoundedFiles = [];
    }

    public void Execute()
    {
        if (!Directory.Exists(Path))
        {
            throw new DirectoryNotFoundException($"Could not find directory at path: {Path}");
        }

        Queue<string> queue = new Queue<string>();
        queue.Enqueue(Path);

        while (queue.Count != 0)
        {
            string directory = queue.Dequeue();
            string[] files = Directory.GetFiles(directory, Mask);
            string[] subdirectories = Directory.GetDirectories(directory);

            foreach (string file in files)
            {
                FoundedFiles.Add(file);
            }

            foreach (string subdir in subdirectories)
            {
                queue.Enqueue(subdir);
            }
        }
    }
}
