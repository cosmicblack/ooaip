using Hwdtech;
using Hwdtech.Ioc;
using Moq;

namespace SpaceBattle.Lib.Test;

public class EndMoveCommandTests
{
    public EndMoveCommandTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
        var mockCommand = new Mock<ICommand>();
        mockCommand.Setup(a => a.Execute());
        var regStrategy = new Mock<IStrategy>();
        regStrategy.Setup(_s => _s.Execute(It.IsAny<object[]>())).Returns(mockCommand.Object);
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Command.EndMovement", (object[] args) => regStrategy.Object.Execute(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.DeleteProperty", (object[] args) => regStrategy.Object.Execute(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Queue.Push", (object[] args) => regStrategy.Object.Execute(args)).Execute();
    }

    [Fact]
    public void PosTestEndMoveCommand()
    {
        var m = new Mock<IMoveCommandEndable>();
        m.SetupGet(a => a.UObject).Returns(new Mock<IUObject>().Object).Verifiable();
        m.SetupGet(a => a.Properties).Returns(new Dictionary<string, object>() { { "speed", new Vector(It.IsAny<int>(), It.IsAny<int>()) } }).Verifiable();
        ICommand EndMoveCommand = new EndMoveCommand(m.Object);
        EndMoveCommand.Execute();
        m.Verify();
    }
    [Fact]
    public void NegTestEndMoveCommand_UnableToGetUObject()
    {
        var m = new Mock<IMoveCommandEndable>();
        m.SetupGet(a => a.UObject).Throws<Exception>().Verifiable();
        m.SetupGet(a => a.Properties).Returns(new Dictionary<string, object>() { { "speed", new Vector(It.IsAny<int>(), It.IsAny<int>()) } }).Verifiable();
        ICommand EndMoveCommand = new EndMoveCommand(m.Object);
        Assert.Throws<Exception>(() => EndMoveCommand.Execute());
    }
    [Fact]
    public void NegTestEndMoveCommand_UnableToGetSpeed()
    {
        var m = new Mock<IMoveCommandEndable>();
        m.SetupGet(a => a.UObject).Returns(new Mock<IUObject>().Object).Verifiable();
        m.SetupGet(a => a.Properties).Throws<Exception>().Verifiable();
        ICommand EndMoveCommand = new EndMoveCommand(m.Object);
        Assert.Throws<Exception>(() => EndMoveCommand.Execute());
    }
}