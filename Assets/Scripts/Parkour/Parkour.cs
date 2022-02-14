using UnityEngine;

public class Parkour : MonoBehaviour
{

  public DetectObs detectVaultObject; // check vault object 
  public DetectObs detectVaultObstruction; // check if wall is in the way
  public DetectObs detectClimbObject; // check climb object
  public DetectObs detectClimbObstruction; // check if a "roof" is in the way

  private float t_parkour;

  private bool canVault;
  public float vaultTime = 0.5f;
  public Transform vaultEndPoint;

  private bool canClimb;
  public float climbTime = 0.5f;
  public Transform climbEndPoint;

  private float parkourMoveTime;
  private bool isParkour;
  private Vector3 moveToPosition;
  private Vector3 startPosition;

  private Rigidbody rb;
  private PlayerMovement pm;

  void Start()
  {
    rb = GetComponent<Rigidbody>();
    pm = GetComponent<PlayerMovement>();
  }

  void Update()
  {
    if (detectVaultObject.Obstruction && !detectVaultObstruction.Obstruction && !canVault
        && !isParkour && (Input.GetButtonDown("Jump")) && !pm.isCrouching())
    {
      canVault = true;
    }

    if (canVault)
    {
      canVault = false;
      rb.isKinematic = true;
      moveToPosition = vaultEndPoint.position;
      startPosition = transform.position;
      isParkour = true;
      parkourMoveTime = vaultTime;

      // add animation
    }

    if (detectClimbObject.Obstruction && !detectClimbObstruction.Obstruction && !canClimb
        && !isParkour && (Input.GetButtonDown("Jump")) && !pm.isCrouching())
    {
      canClimb = true;
    }

    if (canClimb)
    {
      canClimb = false;
      rb.isKinematic = true;
      moveToPosition = climbEndPoint.position;
      startPosition = transform.position;
      isParkour = true;
      parkourMoveTime = climbTime;

      // animation
    }

    if (isParkour && t_parkour < 1f)
    {
      t_parkour += Time.deltaTime / parkourMoveTime;
      transform.position = Vector3.Lerp(startPosition, moveToPosition, t_parkour);

      if (t_parkour >= 1f)
      {
        isParkour = false;
        t_parkour = 0f;
        rb.isKinematic = false;
      }
    }
  }
}
