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

  // crouching
  private Vector3 playerScale;
  private Vector3 crouchScale = new Vector3(1, 0.75f, 1);

  // wall run
  public LayerMask whatIsWall; // currently same as ground
  public float wallRunForce = 3250f;
  public float maxWallRunSpeed = 17f;
  public float maxWallRunCameraTilt = 12f;
  public float wallRunCameraTilt = 0f;
  public float wallRunGravity = 2000f;
  bool isWallRight, isWallLeft, isWallRunning;

  // input
  float x, y;
  bool jumping;

  void Awake()
  {
    rb = GetComponent<Rigidbody>();
  }

  void Start()
  {
    playerScale = transform.localScale;
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

    CheckForWall();
    WallRunInput();
  }


  private void Movement()
  {
    Vector2 mag = FindVelRelativeToLook();
    float xMag = mag.x, yMag = mag.y;

    CounterMovement(x, y, mag);

    Jump();

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

    // limit vertical max speed
    float verticalVelocity = rb.velocity.y;
    float newY = verticalVelocity > maxSpeed ? verticalVelocity * 0.8f : verticalVelocity;
    rb.velocity = new Vector3(rb.velocity.x, newY, rb.velocity.z);
  }

  private void Jump()
  {
    if (jumping && isGrounded())
    {
      rb.AddForce(Vector2.up * jumpForce * 1.5f);
    }
    else if (!isGrounded() && jumping && isWallRunning)
    {
      if (isWallLeft && Input.GetKey(KeyCode.A)) rb.AddForce(orientation.right * jumpForce * 0.75f);
      else if (isWallRight && Input.GetKey(KeyCode.D)) rb.AddForce(-orientation.right * jumpForce * 0.75f);
      else return; // otherwise there is a bug where the player can rocket jump with the wall...

      //Always add forward force
      rb.AddForce(orientation.forward * jumpForce * 0.25f);
      rb.AddForce(Vector2.up * jumpForce * 0.5f);
    }
  }

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
    playerCamera.transform.localRotation = Quaternion.Euler(xRotation, desiredX, wallRunCameraTilt);
    orientation.transform.localRotation = Quaternion.Euler(0, desiredX, 0);

    if (Math.Abs(wallRunCameraTilt) < maxWallRunCameraTilt && isWallRunning && isWallRight)
      wallRunCameraTilt += Time.deltaTime * maxWallRunCameraTilt * 2;
    if (Math.Abs(wallRunCameraTilt) < maxWallRunCameraTilt && isWallRunning && isWallLeft)
      wallRunCameraTilt -= Time.deltaTime * maxWallRunCameraTilt * 2;

    //Tilts camera back again
    if (wallRunCameraTilt > 0 && !isWallRight && !isWallLeft)
      wallRunCameraTilt -= Time.deltaTime * maxWallRunCameraTilt * 2;
    if (wallRunCameraTilt < 0 && !isWallRight && !isWallLeft)
      wallRunCameraTilt += Time.deltaTime * maxWallRunCameraTilt * 2;
  }

  private void UpdateInput()
  {
    x = Input.GetAxisRaw("Horizontal");
    y = Input.GetAxisRaw("Vertical");
    jumping = Input.GetButton("Jump");

    Crouch();
  }

  private void Crouch()
  {
    if (Input.GetKeyDown(KeyCode.LeftControl))
    {
      transform.localScale = crouchScale;
      transform.position = new Vector3(transform.position.x, transform.position.y - 0.25f, transform.position.z);
    }
    else if (Input.GetKeyUp(KeyCode.LeftControl))
    {
      transform.localScale = playerScale;
      // this causes super-jump if the user times the uncrouch and jump correctly
      transform.position = new Vector3(transform.position.x, transform.position.y + 0.25f, transform.position.z);
    }
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

  private void CheckForWall()
  {
    isWallRight = !isGrounded() && Physics.Raycast(transform.position, orientation.right, 1f, whatIsWall);
    isWallLeft = !isGrounded() && Physics.Raycast(transform.position, -orientation.right, 1f, whatIsWall);

    if (!isWallRight && !isWallLeft)
      StopWallRun();
  }

  private void WallRunInput()
  {
    if ((Input.GetKey(KeyCode.D) && isWallRight) || (Input.GetKey(KeyCode.A) && isWallLeft))
      StartWallRun();
  }

  private void StartWallRun()
  {
    rb.useGravity = false;
    isWallRunning = true;

    if (rb.velocity.magnitude <= maxWallRunSpeed)
    {
      rb.AddForce(orientation.forward * wallRunForce * Time.deltaTime);
      rb.AddForce(Vector3.down * wallRunGravity * Time.deltaTime);

      // stick to the wall
      if (isWallRight)
        rb.AddForce(orientation.right * wallRunForce / 5 * Time.deltaTime);
      else
        rb.AddForce(-orientation.right * wallRunForce / 5 * Time.deltaTime);
    }
  }

  private void StopWallRun()
  {
    rb.useGravity = true;
    isWallRunning = false;
  }
}
