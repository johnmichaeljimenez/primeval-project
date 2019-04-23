using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : GenericSingletonClass<CameraManager> {

    public Camera mainCamera;
    public Transform itemFPSContainer;

    Transform target;

    public override void Awake()
    {
        base.Awake();
    }

    void Update()
    {
        if (target)
        {
            transform.position = target.position;
            transform.rotation = target.rotation;
        }
    }

    public void SetTarget(Transform t)
    {
        if (t)
        {
            target = t;
            // transform.localPosition = Vector3.zero;
            // transform.localRotation = Quaternion.identity;

            transform.position = t.position;
            transform.rotation = t.rotation;
        }
    }

    public bool IsTarget(Transform t)
    {
        return target == t;
    }

}
