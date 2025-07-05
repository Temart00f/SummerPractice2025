using System.Reflection;

namespace task07;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property)]
public class DisplayNameAttribute : Attribute
{
    public string DisplayName { get; }
    public DisplayNameAttribute(string displayName) => DisplayName = displayName;
}

[AttributeUsage(AttributeTargets.Class)]
public class VersionAttribute : Attribute
{
    public int Major;
    public int Minor;

    public VersionAttribute(string version)
    {
        string[] temporary = version.Split(".");

        Major = int.Parse(temporary[0]);
        Minor = int.Parse(temporary[1]);
    }
}

[DisplayNameAttribute("Пример класса"), VersionAttribute("1.0")]
public class SampleClass
{
    [DisplayNameAttribute("Числовое свойство")]
    public int Number { get; set; }

    [DisplayNameAttribute("Тестовый метод")]
    public void TestMethod() { }
}

public class ReflectionHelper
{
    public static void PrintTypeInfo(Type type)
    {
        Attribute[] classAtributes = type.GetCustomAttributes().ToArray();

        foreach (Attribute attr in classAtributes)
        {
            if (attr is DisplayNameAttribute displayName)
            {
                Console.WriteLine($"Class Name: \n{displayName.DisplayName}");
            }

            else if (attr is VersionAttribute version)
            {
                Console.WriteLine($"Class Version: \n{version.Major}.{version.Minor}");
            }
        }

        Console.WriteLine("Methods:");

        MethodInfo[] methods = type.GetMethods();

        foreach (MethodInfo method in methods)
        {
            Attribute[] methodAttributes = method.GetCustomAttributes().ToArray();

            foreach (Attribute attr in methodAttributes)
            {
                if (attr is DisplayNameAttribute displayName)
                {
                    Console.WriteLine($"{method.Name} / {displayName.DisplayName}");
                }
            }
        }

        Console.WriteLine("Properties:");

        PropertyInfo[] properties = type.GetProperties();

        foreach (PropertyInfo property in properties)
        {
            Attribute[] propertyAttributes = property.GetCustomAttributes().ToArray();

            foreach (Attribute attr in propertyAttributes)
            {
                if (attr is DisplayNameAttribute displayName)
                {
                    Console.WriteLine($"{property.Name} / {displayName.DisplayName}");
                }
            }
        }
    }
}
