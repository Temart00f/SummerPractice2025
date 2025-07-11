using System.IO;
using System.Reflection;

namespace MetadataMiner
{
    public class MetadataMinerClass
    {
        public static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                throw new FileNotFoundException("Path not specified");
            }

            string dllPath = args[0];

            Assembly asm = Assembly.LoadFrom(dllPath);

            if (asm is null)
            {
                throw new FileNotFoundException("Could not find a file");
            }

            IEnumerable<Type> types = asm.GetTypes().Where(t => t.IsClass);

            Console.WriteLine($"Assembly: {asm.FullName}\n");
            
            foreach (Type type in types)
            { 
                IEnumerable<MethodInfo> methods = type.GetMethods(
                    BindingFlags.Public | 
                    BindingFlags.NonPublic | 
                    BindingFlags.Static | 
                    BindingFlags.Instance | 
                    BindingFlags.DeclaredOnly)
                    .Where(m => !m.Name.StartsWith("get_") && !m.Name.StartsWith("set_"));
                ConstructorInfo[] constructors = type.GetConstructors(
                    BindingFlags.Public | 
                    BindingFlags.NonPublic | 
                    BindingFlags.Static | 
                    BindingFlags.Instance | 
                    BindingFlags.DeclaredOnly);
                IEnumerable<Attribute> attributes = type.GetCustomAttributes();

                Console.WriteLine($"Class {type.Name}:");

                Console.WriteLine("   Methods:");
                foreach (MethodInfo method in methods)
                {
                    ParameterInfo[] methodParameters = method.GetParameters();
                    string methodParametersToOutput = string.Join(", ", methodParameters.Select(p => $"{p.ParameterType.Name} {p.Name}"));
                    Console.WriteLine($"      -{method.Name}({methodParametersToOutput})");
                }

                Console.WriteLine($"   Attributes:");
                foreach (Attribute attribute in attributes)
                {
                    Console.WriteLine($"      -{attribute.GetType().Name}");
                }

                Console.WriteLine("   Constructors:");
                foreach (ConstructorInfo constructor in constructors)
                {
                    ParameterInfo[] constructorParameters = constructor.GetParameters();
                    string constructorParametersToOutput = string.Join(",", constructorParameters.Select(p => $"{p.ParameterType.Name} {p.Name}"));

                    Console.WriteLine($"      -{constructor.Name}({constructorParametersToOutput})");
                }
                Console.WriteLine();
            }
        }
    }
}
