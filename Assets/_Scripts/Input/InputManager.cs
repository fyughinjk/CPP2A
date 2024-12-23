using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    ProjectActions inputs;
    public PlayerController controller;


    protected override void Awake()
    {
        base.Awake();
        inputs = new ProjectActions();
    }

    private void OnEnable()
    {
        inputs.Enable();
        inputs.Movement.Move.performed += controller.OnMove;
        inputs.Movement.Move.canceled += controller.MoveCancelled;

        inputs.Movement.Jump.performed += controller.OnJump;

        inputs.Movement.Attack.performed += controller.OnAttack;

        inputs.Movement.Hands.performed += controller.OnUnEquipWeapon;
        inputs.Movement.Sword.performed += controller.OnEquipSword;
        inputs.Movement.EquipMagic.performed += controller.OnEquipMagic;

    }

    private void OnDisable()
    {
        inputs.Disable();
        inputs.Movement.Move.performed -= controller.OnMove;
        inputs.Movement.Move.canceled -= controller.MoveCancelled;

        inputs.Movement.Jump.performed -= controller.OnJump;

        inputs.Movement.Attack.performed -= controller.OnAttack;

        inputs.Movement.Hands.performed -= controller.OnUnEquipWeapon;
        inputs.Movement.Sword.performed -= controller.OnEquipSword;
        inputs.Movement.EquipMagic.performed -= controller.OnEquipMagic;

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
