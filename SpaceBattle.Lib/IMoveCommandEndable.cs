namespace SpaceBattle.Lib;

public interface IMoveCommandEndable
{
    IUObject UObject { get; }
    ICommand MoveCommand { get; }
    IQueue<ICommand> Queue { get; }
}