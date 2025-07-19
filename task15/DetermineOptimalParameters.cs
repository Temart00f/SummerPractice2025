using System.Diagnostics;
using System.Xml.XPath;
using ScottPlot;
using ScottPlot.AxisRules;
using task14;

class DetermineOptimalParameters
{

    static readonly int functionCallCount = 5;

    static void Main()
    {
        int a = -100;
        int b = 100;
        Func<double, double> function = Math.Sin;
        double[] steps = [1e-1, 1e-2, 1e-3, 1e-4, 1e-5, 1e-6];

        double minimalStep = DetermineMinimalStep_MultiThread(a, b, function, steps);
        (int[], double[], int, double) threadsTimeData = DetermineOptimalThreadCount(a, b, function, minimalStep);
        (double, double) compareData = ComareMultiAndSingle(a, b, function, minimalStep, threadsTimeData.Item4);

        string info = $"Оптимальный шаг: {minimalStep}\n" +
        $"Оптимальное количество потоков: {threadsTimeData.Item3}\n" +
        $"Время многопоточного выполнения: {Math.Round(threadsTimeData.Item4, 4)}\n" +
        $"Время однопоточного выполнения: {Math.Round(compareData.Item1, 4)}\n" +
        $"Разница во времени выполнения в процентах: {Math.Round(compareData.Item2, 2)}%";

        for (int i = 0; i < 15; i++)
        {
            Console.WriteLine($"{threadsTimeData.Item1[i]} : {threadsTimeData.Item2[i]}");
        }

        File.WriteAllText("info.txt", info);

        Plot plot = new();
        plot.Add.Scatter(threadsTimeData.Item2, threadsTimeData.Item1);
        plot.Title("Зависимость времени выполнения от количества потоков");
        plot.XLabel("Время выполнения");
        plot.YLabel("Количество потоков");
        plot.SavePng("graph.png", 800, 600);

    }

    public static double DetermineMinimalStep_MultiThread(double a, double b, Func<double, double> function, double[] steps)
    {
        double minimalStep = steps[0];
        double minimalTime = double.MaxValue;

        foreach (double step in steps)
        {
            double result = 0.0;
            Stopwatch sw = Stopwatch.StartNew();

            for (int i = 0; i < functionCallCount; i++)
            {
                result = DefiniteIntegral.Solve(a, b, function, step, 6);
            }

            sw.Stop();
            double currentAverageTime = sw.Elapsed.TotalMilliseconds / functionCallCount;

            if (currentAverageTime < minimalTime)
            {
                minimalTime = currentAverageTime;
                minimalStep = step;
            }
        }

        return minimalStep;
    }

    public static (int[], double[], int, double) DetermineOptimalThreadCount(double a, double b, Func<double, double> function, double step)
    {
        int[] resultThreadsCount = new int[Environment.ProcessorCount - 1];
        double[] resultThreadsTime = new double[Environment.ProcessorCount - 1];
        int optimalThreads = 0;
        double minimalTime = double.MaxValue;

        for (int i = 2; i < Environment.ProcessorCount + 1; i++)
        {
            Stopwatch sw = Stopwatch.StartNew();

            for (int j = 0; j < functionCallCount; j++)
            {
                DefiniteIntegral.Solve(a, b, function, step, i);
            }

            sw.Stop();
            double currentAverageTime = sw.Elapsed.TotalMilliseconds / functionCallCount;
            resultThreadsCount[i - 2] = i;
            resultThreadsTime[i - 2] = currentAverageTime;

            if (currentAverageTime < minimalTime)
            {
                optimalThreads = i;
                minimalTime = currentAverageTime;
            }
        }

        return (resultThreadsCount, resultThreadsTime, optimalThreads, minimalTime);
    }

    public static (double, double) ComareMultiAndSingle(double a, double b, Func<double, double> function, double step, double MultiThreadAverageTime)
    {
        Stopwatch sw = Stopwatch.StartNew();

        for (int i = 0; i < functionCallCount; i++)
        {
            DefiniteIntegral.IntegrateInSingleThread(a, b, function, step);
        }

        sw.Stop();
        double singleThreadAverageTime = sw.Elapsed.TotalMilliseconds / functionCallCount;
        double precentageDifference = (singleThreadAverageTime - MultiThreadAverageTime) / singleThreadAverageTime * 100;

        return (singleThreadAverageTime, precentageDifference);
    }
}
