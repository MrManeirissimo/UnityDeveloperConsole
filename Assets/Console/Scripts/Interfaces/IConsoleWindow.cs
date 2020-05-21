
using UnityEngine;

namespace DeveloperConsole {

    public interface IResizableWindow {
        Vector2 Position { get; set; }
    }

    public interface IConsoleSensitiveWritter {
        void Log(string line);
        void LogError(string line);
        void LogWarning(string line);
    }
    public interface IConsoleStyledWriter {
        void Write(string[] args, Color color);
        void WriteLine(string line, Color color);
    }
    public interface IConsoleWriter {
        void Write(string[] args);
        void WriteLine(string line);
    }

    public interface IEventSensitiveConsole {
        void ReceivedInput();
    }

    public interface IRemembeableConsole {
        void CacheInput(string input);
        string GetPreviousInput();
        string GetNextInput();
    }

    public interface IConsoleReader {
        string[] Read();
        string ReadLine();
    }


    /// <summary>
    /// Exposes basic funcionality for a console window.
    /// </summary>
    public interface IConsoleWindow : 
        IConsoleWriter,
        IConsoleStyledWriter,
        IConsoleReader,
        IEventSensitiveConsole,
        IResizableWindow {
        void Open();
        void Close();
        void Clear();
    }
}