using PluginLib;

namespace Plugin4;

[PluginLoad([])]
public class Plugin4 : IPlugin
{
    public void Execute()
    {
        Console.WriteLine("Plugin4 was executed");
    }
}
