namespace SpaceBattle.Lib;
using Hwdtech;
public class EndMoveCommand : ICommand
{
    public IMoveCommandEndable obj;
    public EndMoveCommand(IMoveCommandEndable obj)
    {
        this.obj = obj;
    }
    public void Execute()
    {
        ICommand EndCommand = IoC.Resolve<ICommand>("Command.EndMovement", obj.UObject);
        IoC.Resolve<ICommand>("Game.DeleteProperty", obj.UObject, obj.Properties).Execute();
        IoC.Resolve<ICommand>("Queue.Push", obj.Queue, EndCommand).Execute();
    }
}
