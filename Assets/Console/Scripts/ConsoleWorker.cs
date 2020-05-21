using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System;
using UnityEngine;
using System.Text;
using CommandLine;

namespace DeveloperConsole {
    public class ConsoleWorker {
        public static implicit operator bool(ConsoleWorker instance) => instance != null;
        public static ConsoleWorker Instance {
            get {
                if (!instance) {
                    instance = new ConsoleWorker();
                }
                return instance;
            }
        }
        private static ConsoleWorker instance;

        public delegate void ConsoleWorkerLogAction(string log);
        public static event ConsoleWorkerLogAction OnParseFailed;

        Dictionary<string, IConsoleCommand> commandDictionary = new Dictionary<string, IConsoleCommand>();
        IConsoleEnviroment enviroment;

        ConsoleWorker() {
            commandDictionary = new Dictionary<string, IConsoleCommand>();
            enviroment = new ConsoleEnviroment();
            enviroment.AddModule(new Modules.ConsoleWindow(GameObject.FindObjectOfType<ConsoleWindow>()));
            ReloadAssemblies();
        }

        public void Interpret(string line) {
            string[] arguments = SplitCommandLine(line);
            string command = string.Empty;

            (command, arguments) = SeparateCommandFromArguments(arguments);


            if (!commandDictionary.ContainsKey(command)) {
                OnParseFailed(GenerateErrorStatement(command, arguments));
            }
                
            else {
                IConsoleCommand consoleCommand = commandDictionary[command];
                consoleCommand.TryParse(arguments);

                if (consoleCommand.WasParsed()) {
                    consoleCommand.OnParsed(enviroment);
                }
                else {
                    consoleCommand.OnNotParsed(enviroment);
                    OnParseFailed?.Invoke(consoleCommand.GetHelp());
                }
            }
        }

        (string, string[]) SeparateCommandFromArguments(string[] args) => (args[0], args.Where((arg, index) => index != 0).ToArray());
        string[] SplitCommandLine(string line) => line.Split();

        void Parse(IConsoleCommand command, string[] args) {

        }

        public void ReloadAssemblies() {
            var commandTypes = System.AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => typeof(IConsoleCommand).IsAssignableFrom(p) && !p.IsAbstract);

            foreach (var type in commandTypes) {
                foreach (var att in type.GetCustomAttributes(false)) {
                    if (att is CommandAttribute) {
                        commandDictionary.Add((att as CommandAttribute).Command, (IConsoleCommand)System.Activator.CreateInstance(type));
                    }
                }
            }
        }
        public void Clear() {
            commandDictionary.Clear();
        }

        string GenerateErrorStatement(string command, string[] arguments) {
            var errorString = new StringBuilder();
            errorString.Append($"Cannot find command ({command}) in registry. ");
            
            if (arguments.Length > 0) {
                errorString.Append("Arguments: (");
                for (int i = 0; i < arguments.Length; i++) {
                    errorString.Append($"{arguments[i] + (i < arguments.Length - 1 ? ", " : string.Empty)}");
                }
                errorString.Append(") were not parsed.");
            }

            return errorString.ToString();
        }
    }
}
