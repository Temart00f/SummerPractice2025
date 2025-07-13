using Plugin2;
using Plugin3;
using PluginLib;

namespace Plugin1;

[PluginLib.PluginLoad([typeof(Plugin2.Plugin2), typeof(Plugin3.Plugin3)])]
public class Plugin1 : PluginLib.IPlugin
{
    public void Execute()
    {
        Console.WriteLine("Plugin1 was executed");
    }
}
