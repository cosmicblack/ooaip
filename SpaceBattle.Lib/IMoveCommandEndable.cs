namespace SpaceBattle.Lib;

public interface IMoveCommandEndable
{
    IUObject UObject { get; }
    ICommand Command { get; }
    IQueue<ICommand> Queue { get; }
    IDictionary<string, object> Properties
    {
        get;
    }
}