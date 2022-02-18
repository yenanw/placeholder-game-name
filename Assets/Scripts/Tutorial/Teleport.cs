using UnityEngine;

public class Teleport : MonoBehaviour
{
    public GameObject teleportReference;

    public void OnTriggerEnter(Collider Col)
    {
        Col.transform.position = teleportReference.transform.position;
    }
}
