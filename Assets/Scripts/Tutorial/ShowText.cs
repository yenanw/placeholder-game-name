using UnityEngine;

public class ShowText : MonoBehaviour
{

    public Transform player;
    public GameObject text;
    private MeshRenderer textRenderer;
    public float viewDistance = 100f;

    void Start()
    {
        textRenderer = text.GetComponent<MeshRenderer>();
    }

    void Update()
    {
        if (Vector3.Distance(player.position, text.transform.position) > viewDistance)
        {
            textRenderer.enabled = false;
        }
        else
        {
            textRenderer.enabled = true;
        }
    }
}
