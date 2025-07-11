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

public class ClassWithOutAttributes
{
    public int Prop { get; set; }
    public void EmptyMethod() { }
}

public class EmptyClass { }

public class ReflectionHelper
{
    public static void PrintTypeInfo(Type type)
    {
        Attribute[] classAttributes = type.GetCustomAttributes().ToArray();

        if (classAttributes.OfType<DisplayNameAttribute>().Any() == true || classAttributes.OfType<VersionAttribute>().Any() == true)
        {
            foreach (Attribute attr in classAttributes)
            {
                if (attr is DisplayNameAttribute displayName)
                {
                    Console.WriteLine($"Class Name: {displayName.DisplayName}");
                }

                else if (attr is VersionAttribute version)
                {
                    Console.WriteLine($"Class Version: {version.Major}.{version.Minor}");
                }
            }
        }

        else
        {
            Console.WriteLine("Class has no DisplayNameAttribute and VersionAttribute");
        }

        Console.WriteLine("Methods:");

        MethodInfo[] methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

        if (methods.Count() != 0)
        {
            foreach (MethodInfo method in methods)
            {
                Attribute[] methodAttributes = method.GetCustomAttributes().ToArray();

                if (methodAttributes.OfType<DisplayNameAttribute>().Any() == true)
                {
                    foreach (Attribute attr in methodAttributes)
                    {
                        if (attr is DisplayNameAttribute displayName)
                        {
                            Console.WriteLine($"{method.Name} / {displayName.DisplayName}");
                        }
                    }
                }

                else
                {
                    Console.WriteLine($"{method.Name} / Method has no DisplayNameAttribute");
                }
            }
        }

        else
        {
            Console.WriteLine("Class has no methods");
        }

        Console.WriteLine("Properties:");

        PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

        if (properties.Count() != 0)
        {
            foreach (PropertyInfo property in properties)
            {
                Attribute[] propertyAttributes = property.GetCustomAttributes().ToArray();

                if (propertyAttributes.OfType<DisplayNameAttribute>().Any() == true)
                {
                    foreach (Attribute attr in propertyAttributes)
                    {
                        if (attr is DisplayNameAttribute displayName)
                        {
                            Console.WriteLine($"{property.Name} / {displayName.DisplayName}");
                        }
                    }
                }

                else
                {
                    Console.WriteLine($"{property.Name} / Property has no DisplayNameAttribute");
                }
            }
        }

        else
        {
            Console.WriteLine("Class has no properties");
        }
    }
}
