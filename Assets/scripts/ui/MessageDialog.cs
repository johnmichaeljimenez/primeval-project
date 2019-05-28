using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Primeval.ViewModels;

public class MessageDialog : MonoBehaviour
{
    public Button okButton, cancelButton;
    public void Show(string content, string caption = "", UnityAction okAction = null, bool okOnly = true, UnityAction cancelAction = null, string okName = "OK", string cancelName = "Cancel")
    {
        VMMessageBox vm = GetComponent<VMMessageBox>();
        vm.Content = content;
        vm.Caption = caption;

        okButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = okName;
        cancelButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = cancelName;

        okButton.onClick.RemoveAllListeners();
        cancelButton.onClick.RemoveAllListeners();

        cancelButton.gameObject.SetActive(!okOnly);
        if (!okOnly)
        {
            cancelButton.onClick.AddListener(Hide);
            if (cancelAction != null)
                cancelButton.onClick.AddListener(cancelAction);
        }

        okButton.onClick.AddListener(Hide);
        if (okAction != null)
            okButton.onClick.AddListener(okAction);

        gameObject.SetActive(true);
    }

    void Hide()
    {
        gameObject.SetActive(false);
    }
}
