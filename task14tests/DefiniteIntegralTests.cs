using Xunit;
using task14;

namespace task14tests;

public class DefiniteIntegralTests
{
    readonly Func<double, double> X = x => x;
    readonly Func<double, double> SIN = Math.Sin;

    [Fact]
    public void DefiniteIntegral_FuncX_ShouldReturnZero() => Assert.Equal(0, DefiniteIntegral.Solve(-1, 1, X, 1e-4, 2), 1e-4);

    [Fact]
    public void DefiniteIntegral_FuncSin_ShouldReturnZero() => Assert.Equal(0, DefiniteIntegral.Solve(-1, 1, SIN, 1e-5, 8), 1e-4);

    [Fact]
    public void DefiniteIntegral_FuncX_ShouldReturnTwelveAndHalf() => Assert.Equal(12.5, DefiniteIntegral.Solve(0, 5, X, 1e-6, 8), 1e-5);

    [Fact]
    public void DefiniteIntegral_FuncSin_ShouldReturnTwo() => Assert.Equal(2, DefiniteIntegral.Solve(0, Math.PI, SIN, 1e-6, 8), 1e-5);

    [Fact]
    public void DefiniteIntegral_FuncX_ShouldReturnZero_WithSwitchedSegmentBoundaries() => Assert.Equal(0, DefiniteIntegral.Solve(1, -1, X, 1e-4, 2), 1e-4);
}
