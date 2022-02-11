using UnityEngine;

public class RotateHook : MonoBehaviour
{
  public GrapplingHook hook;

  private Quaternion desiredRotation;
  private float rotationSpeed = 5f;

  void Update()
  {
    if (!hook.IsGrappling())
      desiredRotation = transform.parent.rotation;
    else
      desiredRotation = Quaternion.LookRotation(hook.GetGrapplePoint() - transform.position);

    transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, Time.deltaTime * rotationSpeed);
  }
}
