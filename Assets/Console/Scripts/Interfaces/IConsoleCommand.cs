using System.Collections.Generic;
using System;

using CommandLine.Text;
using CommandLine;
using DeveloperConsole.Modules;

namespace DeveloperConsole {
    /// <summary>
    /// 
    /// </summary>
    public interface IConsoleCommand : IDisposable{
        object Result { get; }

        void TryParse(string[] args);
        void OnParsed(IConsoleEnviroment enviroment);
        void OnNotParsed(IConsoleEnviroment enviroment);
        bool WasParsed();
        string GetHelp();
    }

    /// <summary>
    /// 
    /// </summary>
    public abstract class ConsoleCommand : IConsoleCommand {
        public abstract object Result { get; }

        public abstract void OnNotParsed(IConsoleEnviroment enviroment);
        public abstract void OnParsed(IConsoleEnviroment enviroment);

        public abstract void TryParse(string[] args);
        public abstract string GetHelp();
        public abstract bool WasParsed();
        public abstract void Dispose();
    }

    /// <summary>
    /// Base extendable class for creating new console commands.
    /// </summary>
    /// <typeparam name="T">Options class used in the parsing process.</typeparam>
    public abstract class GenericConsoleCommand<T> : IConsoleCommand {
        /// <summary>
        /// The result from the parsing process.
        /// </summary>
        public ParserResult<T> Result { get; private set; }
        object IConsoleCommand.Result => Result;

        public void TryParse(string[] args) => Result = Parser.Default.ParseArguments<T>(args);
        public bool WasParsed() => Result.Tag.Equals(ParserResultType.Parsed);
        public void Dispose() => Result = null;

        void IConsoleCommand.OnNotParsed(IConsoleEnviroment enviroment) => Result.WithNotParsed((err) => OnNotParsed(enviroment, err));
        void IConsoleCommand.OnParsed(IConsoleEnviroment enviroment) => Result.WithParsed((options) => OnParsed(enviroment, options));
        public virtual string GetHelp() {
            return HelpText.AutoBuild(Result, help => {
                help.AdditionalNewLineAfterOption = false;
                help.Copyright = string.Empty;
                help.Heading = string.Empty;
                return help;
            });
        }

        public virtual void OnNotParsed(IConsoleEnviroment enviroment, IEnumerable<Error> err) { }
        public abstract void OnParsed(IConsoleEnviroment enviroment, T options);
    }

    [Command("cls")]
    public class ClsCommand : GenericConsoleCommand<ClsCommand.Options> {
        public class Options {
            [Option('n', "notify", Required = false, HelpText = "Prompts clear message after the clear process.")]
            public bool Notify { get; set; }
        }

        public override void OnParsed(IConsoleEnviroment enviroment, Options options) {
            enviroment.GetModule<Modules.ConsoleWindow>().Window.Clear();
            if (options.Notify)
                enviroment.GetModule<Modules.ConsoleWindow>().Window.WriteLine("> Console cleared");
        }
    }

    [Command("svSetGravity")]
    public class SvSetGravity : GenericConsoleCommand<SvSetGravity.Options> {
        public class Options {
            [Value(0, MetaName = "Gravity", HelpText = "The global value for the game's gravity.", Required = true)]
            public float Gravity { get; set; }
        }

        public override void OnParsed(IConsoleEnviroment enviroment, Options options) {
            // sets  gravity
        }
    }

    [Command("console")]
    public class SetConsole : GenericConsoleCommand<SetConsole.Options> {
        public class Options {
            [Value(0, HelpText = "The subcommand", MetaName = "Subcommand", Required = true)]
            public string Subcommand { get; set; }


            //[Value(1, HelpText = "The x position of the console panel.", MetaName = "X Position", Required = true)]
            [Option('x', Required = false, HelpText = "The X value for transformations")]
            public float X { get; set; }

            //[Value(1, HelpText = "The y position of the console panel.", MetaName = "Y Position", Required = true)]
            [Option('y', Required = false, HelpText = "The X value for transformations")]
            public float Y { get; set; }
        }

        public override void OnParsed(IConsoleEnviroment enviroment, Options options) {
            var command = options.Subcommand;
            if (command == "pos" || command == "position") {
                enviroment.GetModule<Modules.ConsoleWindow>().Window.Position = new UnityEngine.Vector2(options.X, options.Y);
            }
        }
    }
}
