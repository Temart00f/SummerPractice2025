using task17;
using Xunit;
using System.Threading;
using Interfaces;

namespace task17tests;

public class TestCommand : ICommand
{
    private readonly Action Action;

    public TestCommand(Action action) => Action = action;

    public void Execute() => Action();
}

public class ServerThreadTests
{
    [Fact]
    public void ServerThreadTests_IsThreadRunning_ShouldReturnCorrectValue()
    {
        ServerThread serverThread1 = new();
        ServerThread serverThread2 = new();
        bool expected1 = false;
        bool expected2 = true;
        serverThread2.Run();

        Assert.Equal(expected1, serverThread1.IsThreadRunning());
        Assert.Equal(expected2, serverThread2.IsThreadRunning());
    }

    [Fact]
    public void ServerThreadTests_HardStop_StopsInstantly()
    {
        ServerThread serverThread = new();
        bool value = true;

        serverThread.Run();
        HardStop hardStop = new(serverThread);
        TestCommand testCommand = new(() => value = false);

        serverThread.AddCommand(hardStop);
        serverThread.AddCommand(testCommand);

        Thread.Sleep(300);

        Assert.True(value);
    }

    [Fact]
    public void ServerThreadTests_SoftStop_NotBreakingExecutingCommands()
    {
        ServerThread serverThread = new();
        bool value = true;

        serverThread.Run();
        SoftStop softStop = new(serverThread);
        TestCommand testCommand = new(() => value = false);

        serverThread.AddCommand(softStop);
        serverThread.AddCommand(testCommand);

        Thread.Sleep(300);

        Assert.False(value);
    }

    [Fact]
    public void ServerThreadTests_HardStop_ThrowsException_IfExecutedNotFromOwnThread()
    {
        ServerThread serverThread = new();

        serverThread.Run();
        HardStop hardStop = new(serverThread);

        Exception exception = Assert.Throws<Exception>(() => hardStop.Execute());

        Assert.Equal("HardStop cannot be executed on a non-server thread", exception.Message);
    }

    [Fact]
    public void ServerThreadTests_SoftStop_ThrowsException_IfExecutedNotFromOwnThread()
    {
        ServerThread serverThread = new();

        serverThread.Run();
        SoftStop softStop = new(serverThread);

        Exception exception = Assert.Throws<Exception>(() => softStop.Execute());

        Assert.Equal("SoftStop cannot be executed on a non-server thread", exception.Message);
    }
}
