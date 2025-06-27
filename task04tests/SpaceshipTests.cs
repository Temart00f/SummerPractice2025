using Xunit;
using task04;

public class SpaceshipTests
{
    [Fact]
    public void Cruiser_ShouldHaveCorrectStats()
    {
        ISpaceship cruiser = new Cruiser();
        Assert.Equal(50, cruiser.Speed);
        Assert.Equal(500, cruiser.FirePower);
    }

    [Fact]
    public void Fighter_ShouldHaveCorrectStats()
    {
        ISpaceship fighter = new Fighter();
        Assert.Equal(100, fighter.Speed);
        Assert.Equal(250, fighter.FirePower);
    }

    [Fact]
    public void Fighter_ShouldBeFasterThanCruiser()
    {
        var fighter = new Fighter();
        var cruiser = new Cruiser();
        Assert.True(fighter.Speed > cruiser.Speed);
    }

    [Fact]
    public void Cruiser_ShouldMoveForwardCorrect()
    {
        ISpaceship cruiser = new Cruiser();
        string result = cruiser.MoveForward();
        Assert.Equal("Крейсер летит вперед со скоростью 50!", result);
    }

    [Fact]
    public void Fighter_ShouldMoveForwardCorrect()
    {
        ISpaceship fighter = new Fighter();
        string result = fighter.MoveForward();
        Assert.Equal("Истребитель летит вперед со скоростью 100!", result);
    }

    [Fact]
    public void Cruiser_ShouldRotateCorrect()
    {
        ISpaceship cruiser = new Cruiser();
        string result = cruiser.Rotate(45);
        Assert.Equal("Крейсер совершает поворот на 45°", result);
    }

    [Fact]
    public void Fighter_ShouldRotateCorrect()
    {
        ISpaceship fighter = new Fighter();
        string result = fighter.Rotate(52);
        Assert.Equal("Истребитель совершает поворот на 52°", result);
    }

    [Fact]
    public void Cruiser_ShouldFireCorrect()
    {
        ISpaceship cruiser = new Cruiser();
        string result = cruiser.Fire();
        Assert.Equal("Крейсер выпускает залп и наносит 500 урона", result);
    }

    [Fact]
    public void Fighter_ShouldFireCorrect()
    {
        ISpaceship fighter = new Fighter();
        string result = fighter.Fire();
        Assert.Equal("Истребитель выпускает залп и наносит 250 урона", result);
    }

    [Fact]
    public void Cruiser_ShouldStopCorrect()
    {
        ISpaceship cruiser = new Cruiser();
        string result = cruiser.StopMoving();
        Assert.Equal("Крейсер остановился", result);
    }

    [Fact]
    public void Fighter_ShouldStopCorrect()
    {
        ISpaceship fighter = new Fighter();
        string result = fighter.StopMoving();
        Assert.Equal("Истребитель остановился", result);
    }
}