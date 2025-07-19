using System.Diagnostics;
using ScottPlot;
using Interfaces;

class Informator
{
    static void Main()
    {
        var commands = new ICommand[]
        {
            new VeryFastCommand(),
            new FastCommand(),
            new MediumCommandFirst(),
            new MediumCommandSecond(),
            new SlowCommand(),
            new UnstableCommand()
        };

        var measurements = MeasureCommandsMakeInfo(commands, runs: 10);

        CreatePlot(measurements);
    }

    static (int number, double avgTime)[] MeasureCommandsMakeInfo(ICommand[] commands, int runs)
    {
        var results = new (int, double)[commands.Length];
        var stopwatch = new Stopwatch();
        var rnd = new Random();
        string info = "";

        for (int i = 0; i < commands.Length; i++)
        {
            double totalTime = 0;

            for (int j = 0; j < runs; j++)
            {
                stopwatch.Restart();
                commands[i].Execute();
                stopwatch.Stop();
                totalTime += stopwatch.Elapsed.TotalMilliseconds;

                Thread.Sleep(rnd.Next(1, 5));
            }

            results[i] = (i + 1, totalTime / runs);
            info += $"Команда {i + 1}: среднее время {results[i].Item2:F2} мс\n";
        }

        File.WriteAllText("info.txt", info);

        return results;
    }

    static void CreatePlot((int number, double avgTime)[] measurements)
    {
        double[] numbers = new double[measurements.Length];
        double[] times = new double[measurements.Length];
        
        for (int i = 0; i < measurements.Length; i++)
        {
            numbers[i] = measurements[i].number;
            times[i] = measurements[i].avgTime;
        }

        Plot plot = new();

        plot.Add.Scatter(times, numbers);
        plot.Title("Время выполнения команд");
        plot.XLabel("Среднее время (мс)"); 
        plot.YLabel("Номер команды");

        plot.SavePng("graph.png", 600, 300);
    }
}

class VeryFastCommand : ICommand
{
    public void Execute() => Thread.Sleep(5);
}

class FastCommand : ICommand
{
    public void Execute() => Thread.Sleep(15);
}

class MediumCommandFirst : ICommand
{
    public void Execute() => Thread.Sleep(40);
}

class MediumCommandSecond : ICommand
{
    public void Execute() => Thread.Sleep(50);
}

class SlowCommand : ICommand
{
    public void Execute() => Thread.Sleep(100);
}

class UnstableCommand : ICommand
{
    private Random rnd = new Random();
    public void Execute() => Thread.Sleep(rnd.Next(10, 200));
}
