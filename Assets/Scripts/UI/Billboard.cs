using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Transform cam;

    private void Awake()
    {
        if (cam == null)
            cam = Camera.main.transform;
    }

    private void LateUpdate()
    {
        // always look at the direction the camera is looking
        transform.LookAt(transform.position + cam.forward);
    }
}
