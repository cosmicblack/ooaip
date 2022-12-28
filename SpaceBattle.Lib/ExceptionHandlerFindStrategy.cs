namespace SpaceBattle.Lib;

using Hwdtech;

public class ExceptionHandlerFindStrategy : IStrategy
{
    public object Execute(params object[] argv)
    {
        ICommand command = (ICommand)argv[0];
        Exception exception = (Exception)argv[1];

        var ExceptionHandlers = IoC.Resolve<IDictionary<ICommand, IDictionary<Exception, IStrategy>>>("Handler.Exception");

        return ExceptionHandlers[command][exception];
    }
}
