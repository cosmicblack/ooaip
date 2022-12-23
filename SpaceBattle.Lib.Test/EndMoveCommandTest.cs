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
    public void EndMoveCommandTest()
    {
        var mve = new Mock<IMoveCommandEndable>();
        mve.SetupGet(x => x.UObject).Returns(new Mock<IUObject>().Object).Verifiable();
        mve.SetupGet(x => x.Properties).Returns(new Mock<IDictionary<string, object>>().Object).Verifiable();
        ICommand command = new EndMoveCommand(mve.Object);
        command.Execute();
        mve.Verify();
    }
    [Fact]
    public void UnableToGetUObject()
    {
        var mve = new Mock<IMoveCommandEndable>();
        mve.SetupGet(x => x.Command).Returns(new Mock<ICommand>().Object).Verifiable();
        mve.SetupGet(x => x.UObject).Throws<Exception>().Verifiable();
        mve.SetupGet(x => x.Properties).Returns(new Mock<IDictionary<string, object>>().Object).Verifiable();
        ICommand command = new EndMoveCommand(mve.Object);
        Assert.Throws<Exception>(() => command.Execute());
    }
    [Fact]
    public void UnableToGetProperties()
    {
        var mve = new Mock<IMoveCommandEndable>();
        mve.SetupGet(x => x.Command).Returns(new Mock<ICommand>().Object).Verifiable();
        mve.SetupGet(x => x.UObject).Returns(new Mock<IUObject>().Object).Verifiable();
        mve.SetupGet(x => x.Properties).Throws<Exception>().Verifiable();
        ICommand command = new EndMoveCommand(mve.Object);
        Assert.Throws<Exception>(() => command.Execute());
    }
    [Fact]
    public void UnableToGetCommand()
    {
        var mve = new Mock<IMoveCommandEndable>();
        mve.SetupGet(x => x.Command).Throws<Exception>().Verifiable();
        mve.SetupGet(x => x.UObject).Returns(new Mock<IUObject>().Object).Verifiable();
        mve.SetupGet(x => x.Properties).Throws<Exception>().Verifiable();
        ICommand command = new EndMoveCommand(mve.Object);
        Assert.Throws<Exception>(() => command.Execute());
    }
}