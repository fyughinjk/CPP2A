using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private CharacterController cc;

    [Header("Speeds")]
    [SerializeField] private float speed = 5f;
    public float GetSpeed() { return speed; }
    public void SetSpeed(float newSpeed) { speed = newSpeed; }

    [SerializeField] public float jumpForce = 5f;
    [SerializeField] private float rotationSpeed = 30f;
 

    private float gravity;
    private float verticalVelocity;
    private Vector2 direction;  // read from input

    private void Start()
    {
        cc = GetComponent<CharacterController>();
        gravity = Physics.gravity.y;
        verticalVelocity = 0f;
    }

    private void OnApplicationFocus(bool focus)
    {
        if (!focus)
            Cursor.lockState = CursorLockMode.None;
        else
            Cursor.lockState = CursorLockMode.Locked;
    }

    // ---- INPUT CALLBACKS ----
    public void OnMove(InputAction.CallbackContext ctx)
    {
        direction = ctx.ReadValue<Vector2>();
    }

    public void MoveCancelled(InputAction.CallbackContext ctx)
    {
        direction = Vector2.zero;
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && cc.isGrounded)
        {
            verticalVelocity = jumpForce;
        }
    }

    private void Update()
    {
        //Camera relative
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;
        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward.Normalize();
        cameraRight.Normalize();

        //Combine input with camera direction
        Vector3 projectedMoveDirection = cameraForward * direction.y + cameraRight * direction.x;

        //Horizontal movement
        float hSpeed = speed * Time.deltaTime;
        Vector3 horizontalMove = projectedMoveDirection * hSpeed;

        //Check ground, handle vertical velocity
        if (cc.isGrounded && verticalVelocity < 0f)
        {
            // If grounded and we were falling or at rest, reset velocity
            verticalVelocity = 0f;
        }

        // Apply gravity every frame
        verticalVelocity += gravity * Time.deltaTime;

        //Combine horizontal + vertical
        Vector3 finalMove = horizontalMove;
        finalMove.y = verticalVelocity * Time.deltaTime;

        //CharacterController move
        cc.Move(finalMove);

        //Rotate to face move direction
        if (projectedMoveDirection.magnitude > 0.1f)
        {
            float timeStep = rotationSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Slerp(transform.rotation,
                                                  Quaternion.LookRotation(projectedMoveDirection),
                                                  timeStep);
        }
    }
}
