
namespace DeveloperConsole {
    [System.AttributeUsage(
        System.AttributeTargets.Class |
        System.AttributeTargets.Struct,
        AllowMultiple = false
    )]
    public class CommandAttribute : System.Attribute {
        public string MetaName { get; set; }
        public string Command { get; private set;}

        public CommandAttribute(string command) => Command = command;
    }
}
