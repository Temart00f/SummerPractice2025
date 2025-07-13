using PluginLib;

namespace Plugin4;

[PluginLib.PluginLoad([])]
public class Plugin4 : PluginLib.IPlugin
{
    public void Execute()
    {
        Console.WriteLine("Plugin4 was executed");
    }
}
