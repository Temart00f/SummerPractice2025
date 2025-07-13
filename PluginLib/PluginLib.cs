namespace PluginLib;

public interface IPlugin
{
    void Execute();
}

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class PluginLoadAttribute : Attribute
{
    public Type[] Dependencies { get; private set; }

    public PluginLoadAttribute(Type[] dependencies)
    {
        Dependencies = dependencies;
    }
}
