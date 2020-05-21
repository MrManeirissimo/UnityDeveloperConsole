using UnityEngine;
using TMPro;

namespace DeveloperConsole {
    public class ConsoleLine : MonoBehaviour, IConsoleLine {
        public string Text { get => textMesh.text; set => textMesh.text = value; }
        public Color Color { get => textMesh.color; set => textMesh.color = value; }

        [SerializeField] TextMeshProUGUI textMesh;
    }
}