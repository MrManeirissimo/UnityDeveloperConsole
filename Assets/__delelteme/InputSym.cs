using System.Collections.Generic;
using System.Collections;

using UnityEngine.EventSystems;
using DeveloperConsole;
using UnityEngine;
using TMPro;

namespace Deleteme {
    public class InputSym : MonoBehaviour {
        public KeyCode rcvInputKey = KeyCode.Return;
        public KeyCode secondaryInputKey = KeyCode.KeypadEnter;
        private void Update() {
            if (Input.GetKeyDown(rcvInputKey) || Input.GetKeyDown(secondaryInputKey)) {
                FindObjectOfType<ConsoleWindow>().ReceivedInput();
            }
        }
    }
}