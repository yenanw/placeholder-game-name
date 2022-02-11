using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
  public Transform playerCamera;
  public Transform orientation;
  public Transform feet;

  private Rigidbody rb;

  // look
  private float xRotation;
  private float sensitivity = 50f;
  private float sensitivityMultiplier = 1f;

  // movement
  public float jumpForce = 550f;
  public float movementSpeed = 3500f;
  public float maxSpeed = 20f;
  public LayerMask whatIsGround;

  public float counterMovement = 0.175f;
  private float threshold = 0.01f;

  // input
  float x, y;
  bool jumping;

  void Awake()
  {
    rb = GetComponent<Rigidbody>();
  }

  void Start()
  {
    Cursor.lockState = CursorLockMode.Locked;
    Cursor.visible = false;
  }

  void FixedUpdate()
  {
    Movement();
  }

  void Update()
  {
    UpdateInput();
    Look();
  }

  void Movement()
  {
    Vector2 mag = FindVelRelativeToLook();
    float xMag = mag.x, yMag = mag.y;

    CounterMovement(x, y, mag);

    if (jumping && isGrounded())
    {
      rb.AddForce(Vector2.up * jumpForce * 1.5f);
    }

    //If speed is larger than maxspeed, cancel out the input so you don't go over max speed
    if (x > 0 && xMag > maxSpeed) x = 0;
    if (x < 0 && xMag < -maxSpeed) x = 0;
    if (y > 0 && yMag > maxSpeed) y = 0;
    if (y < 0 && yMag < -maxSpeed) y = 0;

    //Some multipliers
    float multiplier = 1f, multiplierV = 1f;

    // Movement in air
    if (!isGrounded())
    {
      multiplier = 0.5f;
      multiplierV = 0.5f;
    }

    //Apply forces to move player
    rb.AddForce(orientation.transform.forward * y * movementSpeed * Time.deltaTime * multiplier * multiplierV);
    rb.AddForce(orientation.transform.right * x * movementSpeed * Time.deltaTime * multiplier);
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

  private void UpdateInput()
  {
    x = Input.GetAxisRaw("Horizontal");
    y = Input.GetAxisRaw("Vertical");
    jumping = Input.GetButton("Jump");
  }

  private Vector2 FindVelRelativeToLook()
  {
    float lookAngle = orientation.transform.eulerAngles.y;
    float moveAngle = Mathf.Atan2(rb.velocity.x, rb.velocity.z) * Mathf.Rad2Deg;

    float u = Mathf.DeltaAngle(lookAngle, moveAngle);
    float v = 90 - u;

    float magnitue = rb.velocity.magnitude;
    float yMag = magnitue * Mathf.Cos(u * Mathf.Deg2Rad);
    float xMag = magnitue * Mathf.Cos(v * Mathf.Deg2Rad);

    return new Vector2(xMag, yMag);
  }

  private void CounterMovement(float x, float y, Vector2 mag)
  {
    if (!isGrounded() || jumping) return;

    //Counter movement
    if (Math.Abs(mag.x) > threshold && Math.Abs(x) < 0.05f || (mag.x < -threshold && x > 0) || (mag.x > threshold && x < 0))
    {
      rb.AddForce(movementSpeed * orientation.transform.right * Time.deltaTime * -mag.x * counterMovement);
    }
    if (Math.Abs(mag.y) > threshold && Math.Abs(y) < 0.05f || (mag.y < -threshold && y > 0) || (mag.y > threshold && y < 0))
    {
      rb.AddForce(movementSpeed * orientation.transform.forward * Time.deltaTime * -mag.y * counterMovement);
    }

    //Limit diagonal running. This will also cause a full stop if sliding fast and un-crouching, so not optimal.
    if (Mathf.Sqrt((Mathf.Pow(rb.velocity.x, 2) + Mathf.Pow(rb.velocity.z, 2))) > maxSpeed)
    {
      float fallspeed = rb.velocity.y;
      Vector3 n = rb.velocity.normalized * maxSpeed;
      rb.velocity = new Vector3(n.x, fallspeed, n.z);
    }
  }

  private bool isGrounded()
  {
    return Physics.CheckSphere(feet.position, 0.1f, whatIsGround);
  }
}
