using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : CharacterBase
{
    public float speed;

    private float horizontalInput;
    private float verticalInput;

    private PlayerAnimator playerAnim;
    private WeaponManager weaponManager;
    private TrailRenderer[] playerTrails;

    [HideInInspector]
    public bool isSprinting;
   
    private float stamina = 100;
    [SerializeField]
    private int maxStamina = 100;
    [SerializeField]
    private float staminaRegenValue = 4;

    private int moneyAmount;
    [SerializeField]
    private int cooldownAfterStaminaFullySpent = 3;
    private bool isInCooldown;
    [SerializeField]
    private float dashCooldown = 2;
    [SerializeField]
    private float dashSpeed = 2;
    [SerializeField]
    private float dashDuration = 0.1f;
    
    private int defendingStaminaCost = 5;
    private int sprintStaminaCost = 7;
    private int dashStaminaCost = 10;

    private bool isInDash;
    private bool isDashAbilityReady = true;

    private Vector3 raysHeight = new Vector3(0, 4.5f, 0);
    private float raysLength = 3;
    [SerializeField] LayerMask playerCollisions;
    // Start is called before the first frame update
    void Awake()
    {
        playerAnim = GetComponent<PlayerAnimator>();
        characterAnimator =  GetComponent<PlayerAnimator>();
        weaponManager = GetComponent<WeaponManager>();
        playerTrails = GetComponentsInChildren<TrailRenderer>();
    }
    private void Start()
    {
        SetCharacterTrailsActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        GetMovement();
        CheckAnimationState();
        RestoreStamina();        
        
        if (health < 1)
        {
           // gameOver
        }
    }

    void Attack()
    {
        float staminaAttackCost = weaponManager.GetCurrentWeapon().GetComponent<WeaponDataStorage>().GetStaminaCost();
        if (stamina - staminaAttackCost >= 0 && !isAttacking && !isDefending )
        {
            playerAnim.PerformAttackAnim();
            stamina -= staminaAttackCost;
            GetComponentInChildren<WeaponSoundManager>().PlayWeaponAttackSound();            
        }        
    }

    private void Defend()
    {
        if (stamina - defendingStaminaCost * Time.deltaTime >= 0 && !isInDash && !isInCooldown && weaponManager.HasShield())
        {
            stamina -= defendingStaminaCost * Time.deltaTime;
            isDefending = true;

            if (stamina < 2) StartCoroutine(StaminaCooldown());
        }
        else 
        {
            isDefending = false;
        }
    }
    private void Sprint()
    {
        if (stamina - sprintStaminaCost * Time.deltaTime >= 0 && !isAttacking && !isInCooldown)
        {
            stamina -= sprintStaminaCost * Time.deltaTime;
            isSprinting = true;

            if (stamina < 2) StartCoroutine(StaminaCooldown());
        }
        else
        {
            isSprinting = false;
        }
    }
    private void Dash()
    {
        if (stamina - dashStaminaCost > 0  && !isInDash && isDashAbilityReady)
        {
            stamina -= dashStaminaCost;
            speed *= dashSpeed;
            SetCharacterTrailsActive(true);
            isInDash = true;
            StartCoroutine(DashCooldown());
        }
    }
    public override void SpendStamina(float staminaToSpend) => stamina -= staminaToSpend;

    private void RestoreStamina()
    {
        if (stamina < 100 && !isAttacking && !isDefending && !isSprinting)
        {
            stamina += staminaRegenValue * Time.deltaTime;
        }
    }
    public override void ReceiveDamage(int damage)
    {
        if (!isInDash)
        {
            health -= damage;
            if (!isDefending)
            {
                GetComponent<CharacterSoundManager>().PlayHitSound();
                characterAnimator.PerformHitAnim();
            }
        }
    }
    private IEnumerator StaminaCooldown()
    {
        isInCooldown = true;
        yield return new WaitForSeconds(cooldownAfterStaminaFullySpent);
        isInCooldown = false;        
    }
    private IEnumerator DashCooldown()
    {
        isDashAbilityReady = false;
        yield return new WaitForSeconds(dashDuration);
        SetCharacterTrailsActive(false);
        isInDash = false;
        speed /= dashSpeed;
        yield return new WaitForSeconds(dashCooldown);
        isDashAbilityReady = true;
    }
    private void SetCharacterTrailsActive(bool isActive)
    {
        foreach (TrailRenderer trail in playerTrails)
        {
            if (isActive) trail.time = 0.2f;
            else trail.time = 0;
        }
    }
    private void GetMovement()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        verticalInput += GetSprintValue();
        
        CheckCollisionsWithObstacles();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Dash();
        }

        transform.Translate(Vector3.right * horizontalInput * speed * Time.deltaTime);
        transform.Translate(Vector3.forward * verticalInput * speed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, GameObject.FindObjectOfType<MoveCamera>().transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Attack();
        }

        if (Input.GetKey(KeyCode.Mouse1))
        {
            Defend();
        }
        else isDefending = false;

        if (Input.GetKey(KeyCode.LeftShift) && verticalInput > 0)
        {
            Sprint();
        }
        else isSprinting = false;
    }
    private void CheckCollisionsWithObstacles()
    {
        Ray[] rays = new Ray[4];
        rays[0] = new Ray(transform.position + raysHeight, transform.TransformDirection(Vector3.forward));
        rays[1] = new Ray(transform.position + raysHeight, transform.TransformDirection(Vector3.back));
        rays[2] = new Ray(transform.position + raysHeight, transform.TransformDirection(Vector3.right));
        rays[3] = new Ray(transform.position + raysHeight, transform.TransformDirection(Vector3.left));
        for (int i = 0; i < 4; i++)
        {
            Debug.DrawRay(rays[i].origin, rays[i].direction * raysLength, Color.red);
        }
        if (Physics.Raycast(rays[0], raysLength, playerCollisions) && verticalInput > 0)
        {
            verticalInput = 0;
        }
        if (Physics.Raycast(rays[1], raysLength, playerCollisions) && verticalInput < 0)
        {
            verticalInput = 0;
        }
        if (Physics.Raycast(rays[2], raysLength, playerCollisions) && horizontalInput > 0)
        {
            horizontalInput = 0;
        }
        if (Physics.Raycast(rays[3], raysLength, playerCollisions) && horizontalInput < 0)
        {
            horizontalInput = 0;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
      if (collision.gameObject.CompareTag("Coins"))
        {
            moneyAmount += Random.Range(5, 21);
            Debug.Log($"You now have {moneyAmount}");
            Destroy(collision.gameObject);
        }
    }
    public float GetCurrentStaminaPoints()
    {
        return stamina;
    }
    public int GetMaxStaminaPoints()
    {
        return maxStamina;
    }
    public float GetHorizontalInput()
    {
        return horizontalInput;
    }
    public float GetVerticalInput()
    {
        return verticalInput;
    }
    public bool IsStanding()
    {
        if (verticalInput == 0 && horizontalInput == 0)
        {
            return true;
        }
        return false;
    }
    public bool IsInCooldownState()
    {
        return isInCooldown;
    }
    public int GetSprintValue()
    {
        if (isSprinting)
        {
            return 1;
        }
        else return 0;
    }
    public int GetMoneyAmount() => moneyAmount;
}

