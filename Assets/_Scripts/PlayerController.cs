using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    CharacterController cc;
    float speed = 5.0f;
    public float rotationSpeed = 30.0f;

    //cc Variables
    Vector2 direction;
    float gravity;

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
        gravity = Physics.gravity.y;
    }

    private void OnApplicationFocus(bool focus)
    {
        if (!focus) Cursor.lockState = CursorLockMode.None;
        Cursor.lockState = CursorLockMode.Locked;
    }


    public void OnMove(InputAction.CallbackContext ctx)
    {
        direction = ctx.ReadValue<Vector2>();
    }

    public void MoveCancelled(InputAction.CallbackContext ctx)
    {
        direction = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        //Grab out camera forward and right vectors
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;

        //Remove yaw rotation
        cameraForward.y = 0;
        cameraRight.y = 0;

        //normalize the camera vectors so that we don't have any unnecessary rotation we only care about the direction not the magnitude
        cameraForward.Normalize();
        cameraRight.Normalize();
        
        //Projection formula for camera relative movement
        Vector3 projectedMoveDirection = cameraForward * direction.y + cameraRight * direction.x;
        Vector3 desiredMoveDirection = projectedMoveDirection;
        float YVel = (!cc.isGrounded) ? gravity * Time.deltaTime : 0f;
        

        //Move along the projected axis via our speed
        desiredMoveDirection.x *= speed * Time.deltaTime;
        desiredMoveDirection.z *= speed * Time.deltaTime;

        //Add gravity to ourselves
        desiredMoveDirection.y = YVel;

        //our final state for this update
        cc.Move(desiredMoveDirection);
        if (desiredMoveDirection.magnitude > 0)
        {
            float timeStep = rotationSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.LookRotation(projectedMoveDirection), timeStep);
        } 
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        
    }
}
