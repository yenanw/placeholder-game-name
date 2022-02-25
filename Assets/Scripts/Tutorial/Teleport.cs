using UnityEngine;

public class Teleport : MonoBehaviour
{
    public GameObject teleportReference;
    public GameObject grapplingHook;

    public void OnTriggerEnter(Collider Col)
    {
        Col.transform.position = teleportReference.transform.position;
        grapplingHook.GetComponent<GrapplingHook>().StopGrapple();
    }
}
