using System.Threading;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Interfaces;
using task18;
using System.Linq.Expressions;

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

    public class TestCommand : ICommand
    {
        public readonly int _id;
        private int _counter = 0;
        private const int MaxExecutions = 3;

        public TestCommand(int id)
        {
            _id = id;
        }

        public void Execute()
        {
            if (_counter >= MaxExecutions)
                return;
            
            Console.WriteLine($"Поток {_id} - вызов {++_counter}");
            
            if (_counter < MaxExecutions)
                throw new CommandNotCompletedException("Command is not completed");
        }

        public bool IsCompleted => _counter >= MaxExecutions;
    }

public class CommandNotCompletedException : Exception
{
    public CommandNotCompletedException(string message) : base(message) { }
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

    public void Stop()
    {
        if (thread is not null)
        {
            isRunning = false;
            thread.Join();
        }
    }

    protected void ThreadHandler()
    {
        while (isRunning)
        {
            try
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

                if (commands.TryTake(out ICommand? newCommand, 100))
                {
                    scheduler.Add(newCommand);
                }

                if (scheduler.HasCommand())
                {
                    ICommand command = scheduler.Select();
                    try
                    {
                        command.Execute();

                        if (command is TestCommand testCmd && !testCmd.IsCompleted)
                        {
                            scheduler.Add(command);
                        }

                        if (command is ILongRunningCommand longRunningCmd && !longRunningCmd.IsCompleted) //////////////////
                        {
                            scheduler.Add(command);
                        }

                    }
                    catch (CommandNotCompletedException)
                    {
                        scheduler.Add(command);
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
            catch (Exception ex)
            {
                throw new Exception($"Exception in main cycle: {ex.Message}");
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
