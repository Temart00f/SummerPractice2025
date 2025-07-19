using System.Diagnostics;
using Interfaces;
using ScottPlot;
using task17;

class Program
{
    static void Main()
    {
        var measure = MeasureCommandsPerformance();
        CreatePerformancePlot(measure);
    }

    static void CreatePerformancePlot((int id, double avgTime)[] measurements)
    {
        var plt = new Plot();

        double[] ids = measurements.Select(m => (double)m.id).ToArray();
        double[] times = measurements.Select(m => m.avgTime).ToArray();

        plt.Title("Среднее время выполнения команд");
        plt.XLabel("Среднее время (мс)");
        plt.YLabel("Номер команды");

        var bar = plt.Add.Scatter(times, ids);

        plt.SavePng("graph.png", 600, 300);
    }

    static public (int id, double avgTime)[] MeasureCommandsPerformance()
    {
        var commands = new TestCommand[]
        {
            new FastCommand(1),
            new MediumCommand(2),
            new SlowCommand(3),
            new UnstableCommand(4),
            new SlowCommand(5)
        };

        const int executionCount = 3;
        ServerThread serverThread = new();
        serverThread.Run();
        var results = new (int id, double avgTime)[commands.Length];
        var stopwatch = new Stopwatch();
        string text = "";

        var completionFlags = new Dictionary<int, bool>();
        foreach (var cmd in commands)
        {
            completionFlags[cmd.Id] = false;
        }

        for (int i = 0; i < commands.Length; i++)
        {
            double totalTime = 0;
            var currentCommand = commands[i];
            completionFlags[currentCommand.Id] = false;

            for (int j = 0; j < executionCount; j++)
            {
                stopwatch.Restart();
                serverThread.AddCommand(currentCommand);

                while (!currentCommand.IsCompleted)
                {
                    Thread.Sleep(50);
                }

                stopwatch.Stop();
                totalTime += stopwatch.Elapsed.TotalMilliseconds;
                completionFlags[currentCommand.Id] = true;
            }

            results[i] = (currentCommand.Id, totalTime / executionCount);
            string curStr = $"Команда {currentCommand.Id}: среднее время = {results[i].avgTime:F2} мс\n";
            text += curStr;
        }

        serverThread.AddCommand(new HardStop(serverThread));
        serverThread.Stop();

        File.WriteAllText("info.txt", text);

        return results;
    }     
}

class FastCommand : TestCommand
{
    private readonly Random _rnd = new Random();
    
    public FastCommand(int id) : base(id) { }

    public new void Execute() => Thread.Sleep(_rnd.Next(30, 50));
}

class MediumCommand : TestCommand
{
    private readonly Random _rnd = new Random();

    public MediumCommand(int id) : base(id) { }

    public new void Execute() => Thread.Sleep(_rnd.Next(65, 110));
}

class SlowCommand : TestCommand
{
    private readonly Random _rnd = new Random();

    public SlowCommand(int id) : base(id) { }

    public new void Execute() => Thread.Sleep(_rnd.Next(75, 130));
}

class UnstableCommand : TestCommand
{
    private readonly Random _rnd = new Random();

    public UnstableCommand(int id) : base(id) { }

    public new void Execute()
    {
        if (_rnd.NextDouble() < 0.2)
            Thread.Sleep(_rnd.Next(200, 350));
        else
            Thread.Sleep(_rnd.Next(50, 90));
    }
}
