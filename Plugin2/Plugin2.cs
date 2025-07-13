using PluginLib;

namespace Plugin2;

[PluginLoad([typeof(Plugin4.Plugin4)])]
public class Plugin2 : IPlugin
{
    public void Execute()
    {
        Console.WriteLine("Plugin2 was executed");
    }
}
