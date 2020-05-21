using UnityEngine;

namespace DeveloperConsole {

    /// <summary>
    /// Exposes console line functionalities.
    /// </summary>
    public interface IConsoleLine {
        string Text { get; set; }
        Color Color { get; set; }
    }
}
