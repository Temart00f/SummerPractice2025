using System;
using System.Reflection;
using System.Collections.Generic;

namespace task05;

public class ClassAnalyzer
{
    private Type _type;

    public ClassAnalyzer(Type type)
    {
        _type = type;
    }

    public IEnumerable<string> GetPublicMethods()
    {
        return _type
        .GetMethods()
        .Where(m => m.IsPublic)
        .Select(m => m.Name);
    }

    public IEnumerable<string> GetMethodParams(string methodname)
    {
        var method = _type.GetMethod(methodname);

        if (method == null)
        {
            return Enumerable.Empty<string>();
        }

        IEnumerable<string> paramOfParams = method
        .GetParameters()
        .Select(p => (p.IsOut ? "out " : p.ParameterType.IsByRef ? "ref " : "") +
        $"{p.ParameterType.Name} {p.Name}");

        string temporary = $"{method.ReturnType.Name}";

        IEnumerable<string> methodReturnType = new[] { temporary };

        return paramOfParams.Concat(methodReturnType);
    }

    public IEnumerable<string> GetAllFields()
    {
        return _type
        .GetFields(
            BindingFlags.Instance |
            BindingFlags.NonPublic |
            BindingFlags.Public |
            BindingFlags.Static)
        .Select(f => f.Name);
    }

    public IEnumerable<string> GetProperties()
    {
        return _type
        .GetProperties(
            BindingFlags.Instance |
            BindingFlags.NonPublic |
            BindingFlags.Public |
            BindingFlags.Static)
        .Select(p => p.Name);
    }

    public bool HasAttribute<T>() where T : Attribute
    {
        return _type.GetCustomAttributes<T>().Any();
    }
}
