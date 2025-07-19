using System.Threading;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Interfaces;
using task18;

namespace task17;

public class ExceptionHandler : IExceptionHandler
{
    public ICommand Cmd;
    public Exception Ex;

    public ExceptionHandler(ICommand cmd, Exception ex)
    {
        Cmd = cmd;
        Ex = ex;
    }

    public void Handle()
    {
        Console.WriteLine($"Command {Cmd} trew exception {Ex}");
    }
} 

public class HardStop : ICommand
{
    private readonly ServerThread ServerThread;

    public HardStop(ServerThread serverThread)
    {
        ServerThread = serverThread;
    }

    public void Execute()
    {
        if (!ServerThread.IsCurrentThread())
        {
            throw new Exception("HardStop cannot be executed on a non-server thread");
        }

        ServerThread.AddHardStopRequest();
    }
}

public class SoftStop : ICommand
{
    private readonly ServerThread ServerThread;

    public SoftStop(ServerThread serverThread)
    {
        ServerThread = serverThread;
    }

    public void Execute()
    {
        if (!ServerThread.IsCurrentThread())
        {
            throw new Exception("SoftStop cannot be executed on a non-server thread");
        }

        ServerThread.AddSoftStopRequest();
    }
}

public class ServerThread
{
    private Thread? thread;
    private bool isHardStopRequested;
    private bool isSoftStopRequested;
    private bool isRunning = false;
    private readonly BlockingCollection<ICommand> commands = new();
    private readonly IScheduler scheduler = new RoundRobinScheduler();

    public void Run()
    {
        thread = new(ThreadHandler);
        isRunning = true;
        thread.Start();
    }

    protected void ThreadHandler()
    {
        while (isRunning)
        {
            if (isHardStopRequested)
            {
                isRunning = false;
                break;
            }
            else if (isSoftStopRequested && !commands.Any() && !scheduler.HasCommand())
            {
                isRunning = false;
                break;
            }

            if (commands.TryTake(out ICommand? newCommand))
            {
                scheduler.Add(newCommand);
            }

            if (scheduler.HasCommand())
            {
                ICommand command = scheduler.Select();
                try
                {
                    command.Execute();

                    if (command is ILongRunningCommand longRunningCmd && !longRunningCmd.IsCompleted)
                    {
                        scheduler.Add(command);
                    }
                }
                catch (Exception ex)
                {
                    var exceptionHandler = new ExceptionHandler(command, ex);
                    exceptionHandler.Handle();
                }
            }
            else
            {
                Thread.Sleep(10);
            }
        }
    }        

    public void AddCommand(ICommand command)
    {
        commands.Add(command);
    }

    public bool IsCurrentThread()
    {
        return Thread.CurrentThread == thread;
    }

    public void AddHardStopRequest()
    {
        isHardStopRequested = true;
    }

    public void AddSoftStopRequest()
    {
        isSoftStopRequested = true;
    }

    public bool IsThreadRunning()
    {
        return isRunning;
    }
}
