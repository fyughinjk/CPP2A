using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    CharacterController cc;
    float speed = 5.0f;

    //cc Variables
    Vector2 direction;
    float gravity;

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
        gravity = Physics.gravity.y;
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
        Vector3 desiredMoveDirection;
        float YVel = (!cc.isGrounded) ? gravity * Time.deltaTime : 0f;

        desiredMoveDirection = new Vector3(direction.x * speed * Time.deltaTime, YVel, direction.y * speed * Time.deltaTime);
        cc.Move(desiredMoveDirection);
    }
}
