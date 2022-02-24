using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    public float InteractionDistance;

    private Interactable _current = null;
    private LayerMask _layer;

    void Awake()
    {
        _layer = LayerMask.GetMask("Interactable");
    }

    void Update()
    {
        // OH YEAH, the pick up is hard-coded btw lmao
        // TODO: fix it but probably never...
        if (_current != null && Input.GetKeyDown(KeyCode.F))
        {
            _current.Select();
            _current = null;
            return;
        }

        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, InteractionDistance, _layer))
        {
            var selection = hit.transform;
            if (_current == null)
            {
                _current = selection.gameObject.GetComponent<Interactable>();
                _current.OnCursorEnter();
                return;
            }

            if (selection.gameObject.GetComponent<Interactable>() == _current)
                return;
        }

        if (_current != null)
        {
            _current.OnCursorExit();
            _current = null;
        }
    }

}
