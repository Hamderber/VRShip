using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Debugger : MonoBehaviour
{
    private TextMeshPro _console;
    public List<string> consoleLines = new();
    private int _currentConsolePage = 1;

    private void Start()
    {
        _console = GameObject.FindGameObjectWithTag("Console").GetComponentInChildren<TextMeshPro>();
        Log(message: "hello world");
    }
    private void UpdateConsole()
    {
        if (_console == null) return;
        string outputText = "";
        foreach (string line in consoleLines)
        {
            outputText = outputText + line + "\n";
        }
        _console.text = outputText;
        if (_console.textInfo.pageCount != _currentConsolePage)
        {
            _console.pageToDisplay = _console.textInfo.pageCount;
            _currentConsolePage = _console.textInfo.pageCount;
        }
    }

    public void DebugObject(bool debugEnabled, string objectType = "")
    {
        switch (objectType)
        {
            case "HandScript":
                Log(debugEnabled, message: $"Handscript debug todo");
                break;
        } 
    }

    public void Log(bool debugEnabled = true, string startOfMessage = "-", string message = "", string middleOfMessage = "", string endOfMessage = "")
    {
        if (debugEnabled)
        {
            string log = $"{startOfMessage}{message}{middleOfMessage}{endOfMessage}.";
            Debug.Log(log);
            consoleLines.Add(log);
            UpdateConsole();
        }
    }
}
