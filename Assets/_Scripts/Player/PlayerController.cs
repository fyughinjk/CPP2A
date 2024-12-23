using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Animator animator;
    private CharacterController cc;
    [SerializeField] private bool isGrounded;

    [Header("Speeds")]
    [SerializeField] private float speed = 5f;
    public float GetSpeed() { return speed; }
    public void SetSpeed(float newSpeed) { speed = newSpeed; }

    [SerializeField] public float jumpForce = 5f;
    [SerializeField] private float rotationSpeed = 30f;

    public float meleeRange = 2f;
    public float swordDamage = 25f;
    public float punchDamage = 10f;
    public LayerMask enemyLayer;
    public GameObject magicProjectilePrefab;
    public GameObject swordObject;
    public Transform magicSpawnPoint;

    public WeaponUI weaponUI;

    public bool swordUnlocked = false;
    public bool magicUnlocked = false;


    private float gravity;
    private float verticalVelocity;
    private Vector2 direction;  

    public enum WeaponType { Unarmed = 0, Sword = 1, Magic = 2 };
    public WeaponType currentWeapon = WeaponType.Unarmed;

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
            Debug.Log("Player jumped");
        }
    }

    public void OnAttack(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            Attack();
        }
    }

    public void EquipWeapon(WeaponType newWeapon)
    {
        currentWeapon = newWeapon;
        animator.SetInteger("WeaponType", (int)currentWeapon);
    }

    public void OnUnEquipWeapon(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            UnequipWeapon();
        }
    }

    public void OnEquipSword(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            EquipSword();
        }
    }

    public void OnEquipMagic(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            EquipMagic();
        }
    }

    public void EquipSword()
    {
        currentWeapon = WeaponType.Sword;
        swordObject.SetActive(true);
    }

    public void EquipMagic()
    {
        currentWeapon = WeaponType.Magic;
        swordObject.SetActive(false);
    }

    public void UnequipWeapon()
    {
        currentWeapon = WeaponType.Unarmed;
        swordObject.SetActive(false);
    }
    public void Attack()
    {
        if (currentWeapon == WeaponType.Unarmed) { punchAttack();}
        else if (currentWeapon == WeaponType.Sword) { SwordAttack();}
        else if(currentWeapon == WeaponType.Magic) { CastMagic();}

        animator.SetInteger("WeaponType", (int)currentWeapon);
        animator.SetTrigger("AttackTrigger");
    }
    void SwordAttack()
    {
        Vector3 origin = transform.position + transform.forward * 1f;
        float radius = meleeRange;
        Debug.Log("Sword Attack");

        Collider[] hits = Physics.OverlapSphere(origin, radius, enemyLayer);
        foreach (Collider c in hits)
        {
            EnemyHealth enemyHealth = c.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(swordDamage);
            }
        }
    }
    void punchAttack()
    {
        Vector3 origin = transform.position + transform.forward * 1f;
        float radius = meleeRange;
        Debug.Log("Punch Attack");


        Collider[] hits = Physics.OverlapSphere(origin, radius, enemyLayer);
        foreach (Collider c in hits)
        {
            EnemyHealth enemyHealth = c.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(punchDamage);
            }
        }
    }
    void CastMagic()
    {

        GameObject proj = Instantiate(magicProjectilePrefab, magicSpawnPoint.position, magicSpawnPoint.rotation);
        Debug.Log("Magic Attack");

        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentWeapon = WeaponType.Unarmed;
            UpdateWeaponIcon();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && swordUnlocked)
        {
            currentWeapon = WeaponType.Sword;
            UpdateWeaponIcon();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && magicUnlocked)
        {
            currentWeapon = WeaponType.Magic;
            UpdateWeaponIcon();
        }

        //Camera relative
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;
        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward.Normalize();
        cameraRight.Normalize();

        isGrounded = cc.isGrounded;


        //Combine input with camera direction
        Vector3 projectedMoveDirection = cameraForward * direction.y + cameraRight * direction.x;

        //Horizontal movement
        float hSpeed = speed * Time.deltaTime;
        Vector3 horizontalMove = projectedMoveDirection * hSpeed;

        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        float currentSpeed = input.magnitude * speed;

        animator.SetBool("IsGrounded", isGrounded);
        animator.SetFloat("Speed", currentSpeed);

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
    void UpdateWeaponIcon()
    {
        if (weaponUI != null)
        {
            weaponUI.UpdateWeaponIcon(currentWeapon);
        }
    }
    public void UnlockSword()
    {
        swordUnlocked = true;
        Debug.Log("Sword unlocked!");
    }
    public void UnlockMagic()
    {
        magicUnlocked = true;
        Debug.Log("Magic unlocked!");
    }
}
