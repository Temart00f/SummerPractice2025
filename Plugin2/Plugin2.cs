using PluginLib;
using Plugin4;

namespace Plugin2;

[PluginLib.PluginLoad([typeof(Plugin4.Plugin4)])]
public class Plugin2 : PluginLib.IPlugin
{
    public void Execute()
    {
        Console.WriteLine("Plugin2 was executed");
    }
}
