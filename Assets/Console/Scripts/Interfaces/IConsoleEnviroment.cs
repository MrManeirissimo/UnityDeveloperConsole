
namespace DeveloperConsole {
    public interface IConsoleEnviroment {
        T GetModule<T>() where T : IConsoleEnviromentModule;
        void RemoveModule(IConsoleEnviromentModule module);
        void AddModule(IConsoleEnviromentModule module);

        IConsoleEnviromentModule[] GetModules();
    }
}
