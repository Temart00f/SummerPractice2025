using Interfaces;
using task17;
using task18;

namespace task19tests;

public class Task19Tests
{
    [Fact]
    public void TestCommand_ExecutesThreeTimes()
    {
        var command = new TestCommand(1);
        int executionCount = 0;

        while (executionCount < 3)
        {
            try
            {
                command.Execute();
                executionCount++;
            }
            catch (CommandNotCompletedException)
            {

            }
        }

        command.Execute();
        Assert.Equal(3, executionCount);
    }

    [Fact]
    public void CommandProcessor_ExecutesMultipleCommandsInParallel()
    {
        var processor = new ServerThread();
        var command1 = new TestCommand(1);
        var command2 = new TestCommand(2);

        processor.Run();
        processor.AddCommand(command1);
        processor.AddCommand(command2);

        bool completed = SpinWait.SpinUntil(() =>
            command1.IsCompleted && command2.IsCompleted,
            TimeSpan.FromSeconds(2));

        processor.Stop();

        Assert.True(completed);
        Assert.True(command1.IsCompleted);
        Assert.True(command2.IsCompleted);
    }

    [Fact]
    public void Scheduler_HandlesCommandResumption()
    {
        RoundRobinScheduler scheduler = new();
        TestCommand command = new(1);

        scheduler.Add(command);
        Assert.True(scheduler.HasCommand());
        var cmd = scheduler.Select();
        Assert.Same(command, cmd);

        try { cmd.Execute(); } catch (CommandNotCompletedException) { }

        scheduler.Add(cmd);
        Assert.True(scheduler.HasCommand());
    }

    [Fact]
    public void CommandProcessor_StopsGracefully()
    {
        ServerThread processor = new();
        var longCommand = new LongRunningTestCommand(10); // 10 итераций

        processor.Run();
        processor.AddCommand(longCommand);

        Thread.Sleep(100);

        var stopTime = DateTime.Now;
        processor.Stop();
        var stopDuration = DateTime.Now - stopTime;

        Assert.True(stopDuration.TotalMilliseconds < 500);
        Assert.False(longCommand.IsCompleted);
    }

    [Fact]
    public void Processor_HandlesCommandExceptions()
    {
        ServerThread processor = new();
        var faultyCommand = new FaultyCommand();

        processor.Run();
        processor.AddCommand(faultyCommand);

        Thread.Sleep(200);
        processor.Stop();
    }

    private class LongRunningTestCommand : ICommand
    {
        private int _counter = 0;
        private readonly int _maxExecutions;

        public LongRunningTestCommand(int maxExecutions)
        {
            _maxExecutions = maxExecutions;
        }

        public bool IsCompleted => _counter >= _maxExecutions;

        public void Execute()
        {
            if (_counter >= _maxExecutions)
                return;

            _counter++;
            Thread.Sleep(50);
            throw new CommandNotCompletedException("");
        }
    }

    private class FaultyCommand : ICommand
    {
        public void Execute()
        {
            throw new InvalidOperationException("Test failure");
        }
    }
}
