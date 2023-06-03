using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine;

public class EnemyAI : MonoBehaviour, IDamageable
{
    [SerializeField] LayerMask whatIsPlayer;
    [SerializeField] private ParticleSystem explosionSmoke;
    [SerializeField] private Slider healthBar;
    [SerializeField] private float reviewRange, attackRange, maxSpeed, minSpeed;
    [SerializeField] private float wayPointDelay = 3f, startTimeShot = 1f;

    private NavMeshAgent agent;
    private Transform player;
    private Camera mainCamera;
    private WeaponEnemy weapon;
    private Vector3 currentWayPoint;
    
    private float delayTimer = 0f, timeShot = 0f; 
    private float currentHealth = 100;
    private bool playerInReviewRange, playerInAttackRange;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        weapon = GetComponentInChildren<WeaponEnemy>();
        player = GameObject.Find("Player").transform;

        mainCamera = Camera.main;
        agent.speed = minSpeed;
    }

    void Start()
    {        
        SearchWalkPoint();
    }

    void Update()
    {
        playerInReviewRange = Physics.CheckSphere(transform.position, reviewRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        
        if(!playerInReviewRange && !playerInAttackRange)
            Patroling();
        if(playerInReviewRange && !playerInAttackRange)
            ChasePlayer();
        if(playerInReviewRange && playerInAttackRange)
            AttackPlayer();
        
        healthBar.value = currentHealth / 100f;   
    }

    void LateUpdate()
    {
        healthBar.gameObject.transform.LookAt(mainCamera.transform.position);
    }

    void ChasePlayer()
    { 
        agent.SetDestination(player.position);
        agent.speed = maxSpeed;
        transform.LookAt(player.position);
    }

    void AttackPlayer()
    {
        agent.speed = 0;
        transform.LookAt(player.position);

        if(timeShot <= 0)
        {
            weapon.Shooting();
            timeShot = startTimeShot;
        }
        else timeShot -= Time.deltaTime;
    }

    public void ApplayDamage(float damage)
    {
        currentHealth -= damage;

        if(currentHealth <= 0)
        {
            explosionSmoke.Play();
            explosionSmoke.transform.SetParent(null);
            Destroy(gameObject);
        }
    }

    void Patroling()
    {
        if(agent.remainingDistance < agent.stoppingDistance)
        {
            delayTimer += Time.deltaTime;

            if (delayTimer >= wayPointDelay)
            {
                SearchWalkPoint();     
                delayTimer = 0f;
            }
        }
    }
    
    void SearchWalkPoint()
    {
        List<Vector3> validPositions = GetValidPositionsOnNavMesh();
        
        if (validPositions.Count > 0)
        {
            int randomIndex = Random.Range(0, validPositions.Count);
            Vector3 randomPosition = validPositions[randomIndex];
            
            currentWayPoint = randomPosition;
            agent.SetDestination(currentWayPoint);
        }
        else Debug.LogWarning("Не удалось найти допустимую позицию.");
    }

    List<Vector3> GetValidPositionsOnNavMesh()
    {
        List<Vector3> validPositions = new List<Vector3>();
        NavMeshTriangulation navMeshData = NavMesh.CalculateTriangulation();
        
        for (int i = 0; i < navMeshData.vertices.Length; i++)
        {
            Vector3 vertex = navMeshData.vertices[i];
            NavMeshHit hit;
            
            if (NavMesh.SamplePosition(vertex, out hit, 1f, NavMesh.AllAreas))
                validPositions.Add(hit.position);
        }
        
        return validPositions;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, reviewRange);
    }
}
