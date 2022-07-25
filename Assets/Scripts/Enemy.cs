using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;
using UnityEngine.UI;

public class Enemy : CharacterBase
{   
    public GameObject healthBarSliderPrefab;
    private GameObject healthBarSlider;
    private GameObject UICanvas;
    private Vector3 healthBarOffset;
    [SerializeField]
    float healthBarHeight;

    [SerializeField]
    private GameObject coinsPrefab;
    private Vector3 coinsSpawnOffset = new Vector3(0, 0.5f, 0);

    private GameObject player;
    private PlayerController playerController;
    
    private NavMeshAgent agent;
    private NavMeshSurface surface;
    private EnemyAnimator enemyAnimator;
    private float wanderingTime = 3;
    private float currentWanderingTime = 0;
    private float wanderingDistance = 10;
    private bool hasSpottedPlayer;

    private WeaponManagerEnemy weaponManager;
    private CharacterSoundManager soundManager;
    [SerializeField]
    private float attackRange = 10f;
    [SerializeField] 
    private float spottingDistance = 45f;
    private Vector3 raysHieght = new Vector3(0, 4, 0);
    private bool isAttackReady = true;
    [SerializeField]
    private float attackCooldown = 1f;

    [SerializeField]
    private float destructionTime = 4;
    public enum EnemyType
    {
        Human,
        Slime
    }
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerController>().gameObject;
        playerController = player.GetComponent<PlayerController>();

        agent = GetComponent<NavMeshAgent>();

        enemyAnimator = GetComponent<EnemyAnimator>();
        characterAnimator = GetComponent<EnemyAnimator>();

        weaponManager = GetComponent<WeaponManagerEnemy>();
        soundManager = GetComponent<CharacterSoundManager>();

        healthBarOffset = new Vector3(0, healthBarHeight, 0);
        UICanvas = GameObject.Find("WorldUI");
        healthBarSlider = Instantiate(healthBarSliderPrefab, UICanvas.transform);             
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHealthBar();
        CastRays();
        UpdateBehaviour();
        UpdateAnimationRelatedVariables();        
        if (health < 1 && isAlive)
        {           
            GetComponent<Collider>().enabled = false;
            if (weaponManager.HasWeapon()) weaponManager.DropWeapon();
            isAlive = false;
            enemyAnimator.PerformDeadAnim();
            Instantiate(coinsPrefab, transform.position + coinsSpawnOffset, coinsPrefab.transform.rotation);
            soundManager.PlayDeathSound();
            StartCoroutine(DestroyInTime());          
        }
    }    
    
    public void SetSurface(NavMeshSurface navMeshSurface)
    {
        surface = navMeshSurface;
    }
    private void UpdateBehaviour()
    {
        if (isPlayerInAttackRange())
        {
            hasSpottedPlayer = true;
            transform.LookAt(player.transform);
            Attack();            
        }
        if (hasSpottedPlayer) FollowPlayer();
        else
        {
            //FollowPlayer();
            Wander();
        }
    }

    private void UpdateAnimationRelatedVariables()
    {
        if (enemyAnimator.GetCurrentAnimation().IsTag("Attack") && isAlive)
        {
            isAttacking = true;
        }
        else
        {
            isAttacking = false;
        }
    }
    private bool isPlayerInAttackRange()
    {
        Collider[] objectsOverlapped = Physics.OverlapSphere(transform.position, attackRange);
        for (int i = 0; i < objectsOverlapped.Length; i++)
        {
            if (objectsOverlapped[i].gameObject.CompareTag("Player")) return true;
        }
        return false;
    }
    private void CastRays()
    {
        Ray ray = new Ray(transform.position + raysHieght, transform.TransformDirection(Vector3.forward));
        if (Physics.Raycast(ray, out RaycastHit hit, spottingDistance) && hit.collider.CompareTag("Player"))
        {
            hasSpottedPlayer = true;
        }
        Debug.DrawRay(ray.origin, ray.direction * spottingDistance);
    }
    private void Attack()
    {        
        //FollowPlayer();
        if (isAttackReady && isAlive)
        {
            isAttacking = true;
            enemyAnimator.PerformAttackAnim();
            GetComponentInChildren<WeaponSoundManager>().PlayWeaponAttackSound();
            StartCoroutine(AttackCooldown());
        }
    }
    private void Wander()
    {
        currentWanderingTime += Time.deltaTime;
        if (currentWanderingTime > wanderingTime)
        {
            Vector3 randDirection = Random.insideUnitSphere * wanderingDistance;

            randDirection += transform.position;

            NavMeshHit navHit;

            NavMesh.SamplePosition(randDirection, out navHit, wanderingDistance, -1);
            
            agent.SetDestination(navHit.position);
            currentWanderingTime = 0;            
        }
    }
    private void FollowPlayer()
    {
        if (isAlive)
        {
            agent.SetDestination(player.transform.position);            
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    private void UpdateHealthBar()
    {
        healthBarSlider.transform.position = transform.position + healthBarOffset;
        healthBarSlider.transform.rotation = transform.rotation;        
        healthBarSlider.GetComponent<Slider>().value = (float)health / 100; 
    }

    private IEnumerator AttackCooldown()
    {
        isAttackReady = false;
        yield return new WaitForSeconds(attackCooldown);
        isAttackReady = true;
    }
    private IEnumerator DestroyInTime()
    {
        yield return new WaitForSeconds(destructionTime);
        Destroy(healthBarSlider);
        Destroy(gameObject);
    }   
}
