using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Reflection;

namespace task11;

public interface ICalculator
{
    public int Add(int a, int b);
    public int Minus(int a, int b);
    public int Mul(int a, int b);
    public int Div(int a, int b);
}

public class CalculatorCreator
{
    public static string? StringToCreate { get; private set; }

    public CalculatorCreator(string stringToCreate)
    {
        StringToCreate = stringToCreate;
    }

    public Type Create()
    {
        SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(StringToCreate!);

        MetadataReference[] references =
        [
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(ICalculator).Assembly.Location)
        ];

        CSharpCompilation compilation = CSharpCompilation.Create(
            "CalculatorAssembly",
            [syntaxTree],
            references,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        using (var ms = new MemoryStream())
        {
            var result = compilation.Emit(ms);

            if (!result.Success)
            {
                throw new Exception("Compilation error");
            }

            ms.Seek(0, SeekOrigin.Begin);
            Assembly assembly = Assembly.Load(ms.ToArray());
            return assembly.GetType("Calculator")!;
        }
    }
}
