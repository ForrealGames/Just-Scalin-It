using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WallMovement : MonoBehaviour
{
    [Header("Wallrunning")]
    public LayerMask wallLayer;
    public LayerMask groundLayer;
    public float wallRunForce;
    public float maxWallRunTime;

    [Header("Input")]
    private float horizontalInput;
    private float verticalInput;

    [Header("Detection")]
    public float wallCheckDistance;
    public float minJumpHeight;
    private RaycastHit leftWallHit;
    private RaycastHit rightWallhit;
    private bool wallLeft;
    private bool wallRight;
    public float yourHeightOffset = 3f;

    [Header("References")]
    public Transform orientation;
    private PlayerController pc;
    private Rigidbody rb;
    public Animator Animator;
    public AudioSource wallRunAudio; // Add this line
    public GameObject wallRunSparksPrefab; // Add this line
    private GameObject wallRunSparks; // Add this line


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        pc = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckForWall();
        StateMachine();
    }

    private void FixedUpdate()
    {
        if (pc.wallRunning)
            wallRunningMovement();
    }

    private void CheckForWall()
    {
        Vector3 raycastOrigin = transform.position + Vector3.up * yourHeightOffset;

        Debug.DrawRay(raycastOrigin, -orientation.right * wallCheckDistance, Color.red);
        Debug.DrawRay(raycastOrigin, orientation.right * wallCheckDistance, Color.blue);

        wallRight = Physics.Raycast(raycastOrigin, -orientation.right, out rightWallhit, wallCheckDistance, wallLayer);
        wallLeft = Physics.Raycast(raycastOrigin, orientation.right, out leftWallHit, wallCheckDistance, wallLayer);

        if (wallRight || wallLeft)
        {
            // Check if sparks prefab is not instantiated
            if (wallRunSparks == null)
            {
                // Instantiate sparks prefab with a height offset of 0.2
                Vector3 sparkSpawnPosition = transform.position + Vector3.up * 0.2f;
                wallRunSparks = Instantiate(wallRunSparksPrefab, sparkSpawnPosition, Quaternion.identity);
                wallRunSparks.transform.parent = transform;
            }
        }
        else
        {
            // Destroy sparks prefab if it's instantiated
            if (wallRunSparks != null)
            {
                Destroy(wallRunSparks);
            }
        }
    }

    private void StateMachine()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        if ((wallLeft || wallRight) && verticalInput > 0)
        {
            if (!pc.wallRunning)
                Debug.Log("startwallrun");
                StartWallRun();
        }

        else
        {
            if (pc.wallRunning)
                StopWallRun();
        }
    }

    private void StartWallRun()
    {
        pc.wallRunning = true;

        // Play the wall run sound effect
        if (wallRunAudio != null)
        {
            wallRunAudio.Play();
        }

        if (wallRight)
        {
            Animator.SetBool("IsWallRunningLeft", true);
            // Set any other parameters or perform actions specific to wall running on the left side
        }
        else if (wallLeft)
        {
            Animator.SetBool("IsWallRunningRight", true);
            // Set any other parameters or perform actions specific to wall running on the right side
        }
    }


    private void wallRunningMovement()
    {
        rb.useGravity = false;
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        Vector3 wallNormal = wallRight ? rightWallhit.normal : leftWallHit.normal;

        Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);

        if ((orientation.forward - wallForward).magnitude > (orientation.forward - -wallForward).magnitude)
            wallForward = -wallForward;

        rb.AddForce(wallForward * wallRunForce, ForceMode.Force);
    }

    private void StopWallRun()
    {
        rb.useGravity = true;
        pc.wallRunning = false;
        pc.isJumping = false;
        pc.jumpForce = 16f;
        Animator.SetBool("IsWallRunningLeft", false);
        Animator.SetBool("IsWallRunningRight", false);
    }

}
