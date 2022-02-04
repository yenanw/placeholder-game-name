using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
  public Transform playerCamera;
  public Transform orientation;
  public Transform feet;

  private Rigidbody rb;

  private float xRotation;
  private float sensitivity = 50f;
  private float sensitivityMultiplier = 1f;

  public float jumpForce = 10f;
  public float movementSpeed = 1300f;
  public LayerMask whatIsGround;

  void Awake()
  {
    rb = GetComponent<Rigidbody>();
  }

  void Start()
  {
    Cursor.lockState = CursorLockMode.Locked;
    Cursor.visible = false;
  }

  void Update()
  {
    Look();
    Movement();
  }

  void Movement()
  {
    rb.AddForce(orientation.transform.forward * Input.GetAxisRaw("Vertical") * movementSpeed * Time.deltaTime);
    rb.AddForce(orientation.transform.right * Input.GetAxisRaw("Horizontal") * movementSpeed * Time.deltaTime);
    if (Input.GetKeyDown(KeyCode.Space))
    {
      if (Physics.CheckSphere(feet.position, 0.1f, whatIsGround))
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
  }

  // this code I just ctrl-c ctrl-v
  private void Look()
  {
    float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.fixedDeltaTime * sensitivityMultiplier;
    float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.fixedDeltaTime * sensitivityMultiplier;

    //Find current look rotation
    Vector3 rot = playerCamera.transform.localRotation.eulerAngles;
    float desiredX = rot.y + mouseX;

    //Rotate, and also make sure we dont over- or under-rotate.
    xRotation -= mouseY;
    xRotation = Mathf.Clamp(xRotation, -90f, 90f);

    //Perform the rotations
    playerCamera.transform.localRotation = Quaternion.Euler(xRotation, desiredX, 0);
    orientation.transform.localRotation = Quaternion.Euler(0, desiredX, 0);
  }
}
