
namespace DeveloperConsole.Modules {
    public class ConsoleWindow : IConsoleEnviromentModule {
        public DeveloperConsole.ConsoleWindow Window { get; }

        public ConsoleWindow(DeveloperConsole.ConsoleWindow window) => Window = window;
    }
}