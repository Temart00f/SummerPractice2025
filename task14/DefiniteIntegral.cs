using System.Threading;

namespace task14;

public class DefiniteIntegral
{
    public static double Solve(double a, double b, Func<double, double> function, double step, int threadsnumber)
    {
        if (a > b) return Solve(b, a, function, step, threadsnumber) * -1;

        double result = 0.0;
        object lockObj = new object();
        double subintervalsLength = (b - a) / threadsnumber;
        using Barrier barrier = new(threadsnumber + 1);

        for (int i = 0; i < threadsnumber; i++)
        {
            double subintervalsStart = a + i * subintervalsLength;
            double subintervalsEnd;
            if (i == threadsnumber - 1)
            {
                subintervalsEnd = b;
            }
            else
            {
                subintervalsEnd = subintervalsStart + subintervalsLength;
            }

            Thread thread = new Thread(() =>
            {
                double subintervalValue = IntegrateSuninterval(subintervalsStart, subintervalsEnd, function, step);
                lock (lockObj)
                {
                    result += subintervalValue;
                }
                barrier.SignalAndWait();
            });

            thread.Start();
        }

        barrier.SignalAndWait();
        return result;
    }

    private static double IntegrateSuninterval(double a, double b, Func<double, double> function, double step)
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
}
