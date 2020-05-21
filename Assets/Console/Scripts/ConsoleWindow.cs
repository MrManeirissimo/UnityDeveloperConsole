using System.Collections.Generic;
using System.Collections;
using System.Text;

using UnityEngine;
using TMPro;

namespace DeveloperConsole {
    public class ConsoleWindow : MonoBehaviour, IConsoleWindow {
        public delegate void ConsoleAction(string input);
        public static event ConsoleAction OnConsoleReceivedInput;

        public Vector2 Position {
            get {
                RectTransform rectTransform = (RectTransform)MainPanel.transform;
                return new Vector2(rectTransform.rect.left, rectTransform.rect.bottom);
            }

            set {
                RectTransform rectTransform = (RectTransform)MainPanel.transform;
                Vector2 size = rectTransform.rect.size;
                rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, value.x, rectTransform.rect.width);
                rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, value.y, rectTransform.rect.height);
                //rectTransform.anchoredPosition = new Vector2(value.x, value.y);
            }
        }

        GameObject MainPanel => transform.GetChild(0).gameObject;
        IConsoleLine LastEntry => lineList[LastEntryIndex];
        int LastEntryIndex => lineList.Count - 1;

        [SerializeField] GameObject sampleLine;
        [SerializeField] Transform content;
        [SerializeField] TMP_InputField inputField;

        List<IConsoleLine> lineList = new List<IConsoleLine>();

        private void OnValidate() {
            if ((sampleLine.GetComponent<IConsoleLine>() == null)) {
                Debug.LogError($"Suplied 'sampleLine' is not derived from {typeof(IConsoleLine).Name}." +
                    $" Component may not work properly.");
            }
        }

        private void OnDisable() {
            ConsoleWorker.OnParseFailed -= ConsoleWorker_OnParseFailed;
        }

        private void OnEnable() {
            ConsoleWorker.OnParseFailed += ConsoleWorker_OnParseFailed;
        }

        private void Start() {
            WriteLine("Developer Console [Version 10.0.17134.1488]");
            WriteLine("(c) 2020 Pazze & Sflofler Corporation. All rights reserved.");
            inputField.Select();
        }

        public string[] Read() {
            throw new System.NotImplementedException();
        }

        public string ReadLine() {
            throw new System.NotImplementedException();
        }

        public void Write(string[] args, Color color) => WriteLine(BuildLineFromArray(args), color);
        public void Write(string[] args) => WriteLine(BuildLineFromArray(args));

        public void WriteLine(string line) => CreateLine().Text = line;
        public void WriteLine(string textLine, Color color) {
            IConsoleLine line = CreateLine();
            line.Text = textLine;
            line.Color = color;
        }

        [ContextMenu("Open console")] public void Open() => MainPanel.SetActive(true);
        [ContextMenu("Close console")] public void Close() => MainPanel.SetActive(false);
        [ContextMenu("Clear console")] public void Clear() {
            for (int i = lineList.Count - 1; i >= 0; i--) {
                Destroy((lineList[i] as MonoBehaviour).gameObject);
                lineList.RemoveAt(i);
            }
        }

        public void ReceivedInput() {
            if (!string.IsNullOrEmpty(inputField.text)) {
                WriteLine($"> {inputField.text}");

                ConsoleWorker.Instance.Interpret(inputField.text);
                inputField.text = string.Empty;
            }
        }

        void ConsoleWorker_OnParseFailed(string log) {
            WriteLine(log, Color.red);
        }

        string BuildLineFromArray(string[] args) {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < args.Length; i++)
                builder.Append(args[i] + (i < args.Length - 1 ? " " : string.Empty));

            return builder.ToString();
        }

        IConsoleLine CreateLine() {
            lineList.Add(Instantiate(sampleLine, content).GetComponent<IConsoleLine>());
            return lineList[LastEntryIndex];
        }
    }
}
