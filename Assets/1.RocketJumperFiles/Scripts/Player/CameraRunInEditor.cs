using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraRunInEditor : CameraController
{
    // Update is called once per frame
    void Update()
    {
        CameraSizeController();
        CameraPositionController();
    }
}
