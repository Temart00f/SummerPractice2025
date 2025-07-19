using System.Threading;

namespace task14;

public class DefiniteIntegral
{
    public static double Solve(double a, double b, Func<double, double> function, double step, int threadsnumber)
    {
        if (a > b) return Solve(b, a, function, step, threadsnumber) * -1;

        threadsnumber = Math.Min(threadsnumber, Environment.ProcessorCount);
        double[] partialResults = new double[threadsnumber];

        Parallel.For(0, threadsnumber, i =>
        {
            double subintervalStart = a + i * (b - a) / threadsnumber;
            double subintervalEnd = a + (i + 1) * (b - a) / threadsnumber;
            partialResults[i] = IntegrateSubinterval(subintervalStart, subintervalEnd, function, step);
        });

        return partialResults.Sum();
    }

    public static double IntegrateSubinterval(double a, double b, Func<double, double> function, double step)
    {
        int n = (int)Math.Ceiling((b - a) / step);
        step = (b - a) / n;
        double sum = 0;
        double x = a;

        for (int i = 0; i < n; i++)
        {
            double nextx = x + step;
            sum += (function(x) + function(nextx)) * 0.5 * step;
            x = nextx;
        }

        return sum;
    }

    public static double IntegrateInSingleThread(double a, double b, Func<double, double> function, double step)
    {
        double result = 0.0;

        for (double x = a; x < b; x += step)
        {
            double nextx = Math.Min(x + step, b);
            result += (function(x) + function(nextx)) * 0.5 * (nextx - x);
        }

        return result;
    }
}
