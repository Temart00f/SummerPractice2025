namespace Interfaces;

public interface IScheduler
{
    bool HasCommand();
    ICommand Select();
    void Add(ICommand cmd);
}

public interface ILongRunningCommand
{
    bool IsCompleted { get; }
}

public interface ICommand
{
    void Execute();
}

public interface IExceptionHandler
{
    void Handle();
}
