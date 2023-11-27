using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    public Transform orientation;
    public Rigidbody rb;
    public Animator animator;

    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 15f;
    public float wallRunSpeed;
    public bool isJumping;
    
    Vector3 moveDirection;

    public MovementState state;
   
    public enum MovementState
    {
        wallrunning
    }

    public bool wallRunning;

    void Start()
    {
        rb.freezeRotation = true;
    }

    void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        
        //Sets move direction to the rotation of the player
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        //sets velocity to the movement speed on y and z axis
        rb.velocity = new Vector3(moveDirection.normalized.x * moveSpeed, rb.velocity.y, moveDirection.normalized.z * moveSpeed);

        Debug.Log(isJumping);

        if (!isJumping && moveDirection.magnitude > 0.1f)
        {
            animator.SetBool("IsRunning", true);
        }
        else
        {
            animator.SetBool("IsRunning", false);
        }

        if (Gamepad.current != null && Gamepad.current.aButton.wasPressedThisFrame)
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            Jump();
        }
    }

    void Jump()
    {
        if (!isJumping)
        {
            animator.SetBool("Jump", true);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isJumping = true;
        }
    }

    public void StateHandler()
    {
        if (wallRunning)
        {
            state = MovementState.wallrunning;
            moveSpeed = wallRunSpeed;
        }
    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Platform"))
        {
            isJumping = false;
            animator.SetBool("Jump", false);
            jumpForce = 15.5f;
        }
        else
        {
            isJumping = true;
        }

        if (collision.gameObject.CompareTag("WinnerCircle"))
        {
            isJumping = false;
            animator.SetBool("Jump", false);
            SuccessUI successUIScript = FindObjectOfType<SuccessUI>();
            if (successUIScript != null)
            {
                successUIScript.ShowSuccessPanel();
            }

            RisingLava risingLavaScript = FindObjectOfType<RisingLava>();
            if (risingLavaScript != null)
            {
                risingLavaScript.PauseLava();
            }

            BackgroundMusic backgroundMusicScript = FindObjectOfType<BackgroundMusic>();
            if (backgroundMusicScript != null)
            {
                backgroundMusicScript.LowerVolume();
            }
        }
    }
}
