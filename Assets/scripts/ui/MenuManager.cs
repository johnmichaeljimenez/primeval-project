using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : GenericSingletonClass<MenuManager>
{
    public MessageDialog messageDialog;
    public LoadingDialog loadingDialog;

    void Awake()
    {
        base.Awake();

        
    }
}
