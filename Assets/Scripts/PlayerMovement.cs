using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

  private Vector2 playerMouseInput;
  private float xRotation;

  public Transform playerCamera;
  public Rigidbody playerBody;
  public Vector3 playerMovementInput;
  public LayerMask floorLayer;
  public Transform feet;

  public float sensitivity = 3f;
  public float movementSpeed = 12f;
  public float jumpForce = 5f;

  void Update()
  {
    playerMovementInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
    playerMouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

    MovePlayer();
    MovePlayerCamera();
  }

  private void MovePlayer()
  {
    Vector3 moveVector = transform.TransformDirection(playerMovementInput) * movementSpeed;
    playerBody.velocity = new Vector3(moveVector.x, playerBody.velocity.y, moveVector.z);

    if (Input.GetKeyDown(KeyCode.Space))
    {
      if (Physics.CheckSphere(feet.position, 0.1f, floorLayer))
        playerBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
  }

  private void MovePlayerCamera()
  {
    xRotation -= playerMouseInput.y * sensitivity;
    transform.Rotate(0f, playerMouseInput.x * sensitivity, 0f);
    playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
  }
}
