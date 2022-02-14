using UnityEngine;

public class DetectObs : MonoBehaviour
{
  public string objectTagname = "";
  public bool obstruction;
  public GameObject obj;
  private Collider currentCollider;

  void OnTriggerStay(Collider col)
  {
    if (objectTagname != "" && !obstruction)
    {
      if (col.GetComponent<CustomTag>())
      {
        if (col.GetComponent<CustomTag>().isEnabled)
        {
          if (col != null && !col.isTrigger && col.GetComponent<CustomTag>().HasTag(objectTagname)) // checks if the object has the right tag
          {
            obstruction = true;
            obj = col.gameObject;
            currentCollider = col;
          }
        }
      }
    }

    if (objectTagname == "" && !obstruction)
    {
      if (col != null && !col.isTrigger)
      {
        obstruction = true;
        currentCollider = col;
      }
    }
  }

  private void Update()
  {
    if (obj == null || !currentCollider.enabled)
    {
      obstruction = false;
    }
    if (obj != null)
    {
      if (!obj.activeInHierarchy)
      {
        obstruction = false;
      }
    }
  }

  void OnTriggerExit(Collider col)
  {
    if (col == currentCollider)
    {
      obstruction = false;
    }
  }
}