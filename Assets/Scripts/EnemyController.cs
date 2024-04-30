using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

public class EnemyController : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    [SerializeField]
    public LayerMask whatIsGround, whatIsPlayer;
    // Start is called before the first frame update
    [SerializeField]
    public int health = 1;
    bool justDied = false;
    bool IsDead = false;
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;
    int random;
    public float timeBetweenAttacks;
    bool alreadyAttacked = false;
    private float timer;
    public float sightRange, attackRange;
     float deathTimer = 0;
    public bool playerInSightRange, playerInAttackRange;
    [SerializeField]
    TMP_Text score;
    void Awake()
    {
        player = GameObject.Find("Paladin J Nordstrom (1)").transform;
        agent = GetComponent<NavMeshAgent>();

    }

    // Update is called once per frame
    void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        if(!playerInSightRange && !playerInAttackRange) Patrolling();
        if(playerInSightRange && !playerInAttackRange) ChasePlayer();
        if(playerInSightRange && playerInAttackRange && alreadyAttacked == false){
            AttackPlayer();
        }
        if(health <= 0)
        {
            GetComponent<Animator>().SetBool("IsDead", true);
            deathTimer += Time.deltaTime;
            if(deathTimer >= 3){
                Destroy(this.gameObject);
                IsDead = false;
            }
        }
    }
    void Patrolling()
    {
        if(!walkPointSet) SearchWalkPoint();
        
        if(walkPointSet) SearchWalkPoint();
        {
            agent.SetDestination(walkPoint);
        }
        GetComponent<Animator>().SetBool("IsMoving", true);
        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if(distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        GetComponent<Animator>().SetBool("IsMoving", false);
        if(Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
        }
    }
    void ChasePlayer()
    {
        agent.SetDestination(player.position);
        GetComponent<Animator>().SetBool("IsMoving", true);
    }
    void AttackPlayer()
    {
        GetComponent<Animator>().SetBool("IsAttacking", true);
        
        agent.SetDestination(transform.position);

        transform.LookAt(player);
        
        if(!alreadyAttacked)
        {
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        GetComponent<Animator>().SetBool("IsAttacking", false);
        alreadyAttacked = false;
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }

    private void OnTriggerStay(Collider other) {
        if(other.gameObject.tag == "Player" && Input.GetMouseButtonDown(0))
        {
            health --;
            justDied = true;
        }
    }
}
