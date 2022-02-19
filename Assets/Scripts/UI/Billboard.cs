using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Transform Camera;

    void LateUpdate()
    {
        // always look at the direction the camera is looking
        transform.LookAt(transform.position + Camera.forward);
    }
}
