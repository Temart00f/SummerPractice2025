using System.Threading;
using System.Collections.Concurrent;

namespace task17;

public interface ICommand
{
    void Execute();
}

public interface IExceptionHandler
{
    void Handle();
}

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
    private BlockingCollection<ICommand> commands = new();

    public void Run()
    {
        thread = new(ThreadHandler);
        isRunning = true;
        thread.Start();
    }

    private void ThreadHandler()
    {
        while (isRunning)
        {
            if (isHardStopRequested)
            {
                isRunning = false;
                break;
            }
            else if (isSoftStopRequested && !commands.Any())
            {
                isRunning = false;
                break;
            }

            ICommand command = commands.Take();

            try
            {
                command.Execute();
            }
            catch (Exception ex)
            {
                ExceptionHandler exceptionHandler = new(command, ex);
                exceptionHandler.Handle();
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


