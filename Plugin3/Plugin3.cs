using PluginLib;

namespace Plugin3;

[PluginLoad([typeof(Plugin4.Plugin4)])]
public class Plugin3 : IPlugin
{
    public void Execute()
    {
        Console.WriteLine("Plugin3 was executed");
    }
}
