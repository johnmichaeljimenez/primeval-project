using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Primeval.ViewModels;

public class UIConsole : MonoBehaviour {

    public int maxLines = 10;

    int lines;
    string messages;

    void Start()
    {
        lines = 0;
        messages = "";
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            transform.GetChild(0).gameObject.SetActive(!transform.GetChild(0).gameObject.activeInHierarchy);
        }
    }

    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        string f1 = "", f2 = "";

        if (type == LogType.Error)
        {
            f1 = "<color=red>";
            f2 = "</color>";
        }

        messages += "> " + f1 + logString + f2 + "\n";
        lines += 1;

        if (lines >= maxLines)
        {
            messages = messages.Substring(messages.IndexOf("\n")+1);
            lines--;
        }


        GetComponentInParent<VMConsole>().Text = messages;
    }
}
