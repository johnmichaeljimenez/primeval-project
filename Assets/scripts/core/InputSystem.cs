using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputSystem : GenericSingletonClass<InputSystem>
{
    //kb

    //mouse
    public static bool mouseRightClicked { get; private set; }
    public static bool mouseRightHold { get; private set; }
    public static bool mouseLeftClicked { get; private set; }
    public static bool mouseLeftHold { get; private set; }
    public static bool overUI { get; private set; }

    public override void Initialize()
    {
        mouseLeftClicked = false;
        mouseLeftHold = false;
        mouseRightClicked = false;
        mouseRightHold = false;
        overUI = false;
    }

    void Update()
    {

        overUI = EventSystem.current.IsPointerOverGameObject();

        mouseLeftClicked = Input.GetMouseButtonDown(0) && !overUI;
        mouseLeftHold = Input.GetMouseButton(0) && !overUI;
        mouseRightClicked = Input.GetMouseButtonDown(1) && !overUI;
        mouseRightHold = Input.GetMouseButton(1) && !overUI;
    }
}