using Interfaces;
using task18;

namespace task18tests;

public class TestCommand : ICommand
{
    public void Execute() { }
}

public class RoundRobinSchedulerTests
{
    [Fact]
    public void Add_AddsCommandToScheduler()
    {
        RoundRobinScheduler scheduler = new();
        TestCommand command = new();

        scheduler.Add(command);

        Assert.True(scheduler.HasCommand());
    }

    [Fact]
    public void Select_ReturnsCommandsInFIFOOrder()
    {
        RoundRobinScheduler scheduler = new();
        TestCommand command1 = new();
        TestCommand command2 = new();

        scheduler.Add(command1);
        scheduler.Add(command2);

        Assert.Same(command1, scheduler.Select());
        Assert.Same(command2, scheduler.Select());
        Assert.False(scheduler.HasCommand());
    }

    [Fact]
    public void Select_ThrowsWhenNoCommands()
    {
        RoundRobinScheduler scheduler = new();

        Exception exception = Assert.Throws<Exception>(() => scheduler.Select());

        Assert.Equal("No commands available", exception.Message);
    }
}
