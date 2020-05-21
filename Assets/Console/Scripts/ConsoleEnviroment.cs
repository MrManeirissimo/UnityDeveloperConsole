using System.Collections.Generic;
using System.Collections;
using System.Net.Configuration;

namespace DeveloperConsole {
    public class ConsoleEnviroment : IConsoleEnviroment {
        public delegate void EnviromentModuleAction(IConsoleEnviromentModule module);
        public static event EnviromentModuleAction OnModuleRemoved;
        public static event EnviromentModuleAction OnModuleAdded;
        public static event EnviromentModuleAction OnModuleRemoveFailed;
        public static event EnviromentModuleAction OnModuleAddFailed;

        List<IConsoleEnviromentModule> modulesList = new List<IConsoleEnviromentModule>();

        public IConsoleEnviromentModule[] GetModules() => modulesList.ToArray();

        public void RemoveModule(IConsoleEnviromentModule module) {
            if (modulesList.Contains(module)) {
                modulesList.Remove(module);
                OnModuleRemoved?.Invoke(module);
            }

            else OnModuleRemoveFailed?.Invoke(module);
        }

        public void AddModule(IConsoleEnviromentModule module) {
            if (!modulesList.Contains(module)) {
                modulesList.Add(module);
                OnModuleAdded?.Invoke(module);
            }

            else OnModuleAddFailed?.Invoke(module);
        }

        public T GetModule<T>() where T : IConsoleEnviromentModule {
            foreach (var module in modulesList)
                if (module is T)
                    return (T)module;

            return default;
        }
    }
}
