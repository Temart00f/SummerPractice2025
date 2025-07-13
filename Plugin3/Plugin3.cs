using PluginLib;
using Plugin4;

namespace Plugin3;

[PluginLib.PluginLoad([typeof(Plugin4.Plugin4)])]
public class Plugin3 : PluginLib.IPlugin
{
    public void Execute()
    {
        Console.WriteLine("Plugin3 was executed");
    }
}
