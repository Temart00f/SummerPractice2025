using Microsoft.CodeAnalysis;
using Xunit;
using task11;

namespace task11tests;

public class CalculatorCreatorTests
{
    [Fact]
    public void CalculatorCreatorTest_SholdCreateClassCorrect()
    {
        var stringToCreate = @"
        public class Calculator : task11.ICalculator
        {
            public int Add(int a, int b) => a + b;
            public int Minus(int a, int b) => a - b;
            public int Mul(int a, int b) => a * b;
            public int Div(int a, int b) => a / b;
        }";

        CalculatorCreator calculator = new(stringToCreate);

        var calculatorClassType = calculator.Create();
        var calculatorObject = Activator.CreateInstance(calculatorClassType) as ICalculator;
        var resultAdd = calculatorObject!.Add(52, 52);
        var resultMinus = calculatorObject!.Minus(52, 52);
        var resultMul = calculatorObject!.Mul(52, 52);
        var resultDev = calculatorObject!.Div(52, 52);
        Assert.Equal(104, resultAdd);
        Assert.Equal(0, resultMinus);
        Assert.Equal(2704, resultMul);
        Assert.Equal(1, resultDev);
    }

    [Fact]
    public void CalculatorCreatorTest_SholdThrowException()
    {
        var stringToCreate = "Uncorrect code";

        CalculatorCreator calculator = new(stringToCreate);

        var exception = Assert.Throws<Exception>(() => { calculator.Create(); });
        Assert.Contains("Compilation error", exception.Message);
    }
}
