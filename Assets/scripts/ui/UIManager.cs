using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIManager : GenericSingletonClass<UIManager>
{
    public MessageDialog messageDialog;
    public LoadingDialog loadingDialog;

    void Awake()
    {
        base.Awake();
    }

    public static void ShowLoading(bool s)
    {
        if (s)
            instance.loadingDialog.Show();
        else
            instance.loadingDialog.Hide();
    }

    public static void ShowMessage(string content, string caption = "", UnityAction okAction = null, bool okOnly = true, UnityAction cancelAction = null)
    {
        instance.messageDialog.Show(content, caption, okAction, okOnly, cancelAction);
    }
}
