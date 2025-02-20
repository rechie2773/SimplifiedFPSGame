using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    public float health;

    public int damage;


    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking    
    public GameObject projectile;
    public float projectileLifetime; //destroy projectile

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    //audio
    public AudioSource audioSource;
    public AudioClip enemyBossSound;
    public bool AudioPlayed = false;
    //audio hitmarker
    public AudioClip hitSound;

    //check enemies/bosses remains
    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (Physics.Raycast(transform.position, player.position - transform.position, out RaycastHit hit, attackRange, whatIsPlayer))
        {
            // Player is within attack range and raycast hit something
            if (hit.collider.CompareTag("Player")) // Check if hit object is the player
            {
                // Apply damage to the player
                PlayerHealth playerHealth = hit.transform.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damage); // Deal damage to the player
                }
            }
        }

        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
            playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

            if (!playerInSightRange && !playerInAttackRange) Patroling();
            if (playerInSightRange && !playerInAttackRange) ChasePlayer();
            if (playerInAttackRange && playerInSightRange) AttackPlayer();

        if (playerInSightRange && CompareTag("EnemyBoss") && !AudioPlayed)
        {
            audioSource.PlayOneShot(enemyBossSound);
            AudioPlayed = true; // mark the audio as played
        }

        else if (!playerInSightRange && AudioPlayed)
        {
            audioSource.Stop();
            AudioPlayed = false; // reset the audio played status when the player leaves sight range
        }


    }

    private void Patroling()
        {
            if (!walkPointSet) SearchWalkPoint();

            if (walkPointSet)
                agent.SetDestination(walkPoint);

            Vector3 distanceToWalkPoint = transform.position - walkPoint;

            //Walkpoint reached
            if (distanceToWalkPoint.magnitude < 1f)
                walkPointSet = false;
        }
        private void SearchWalkPoint()
        {
            //Calculate random point in range
            float randomZ = Random.Range(-walkPointRange, walkPointRange);
            float randomX = Random.Range(-walkPointRange, walkPointRange);

            walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

            if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
                walkPointSet = true;
        }

        private void ChasePlayer()
        {
            agent.SetDestination(player.position);
        }

        private void AttackPlayer()
        {
            //Make sure enemy doesn't move
            agent.SetDestination(transform.position);

            transform.LookAt(player);
                ///Attack 
                Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
                rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
                rb.AddForce(transform.up * 8f, ForceMode.Impulse);
                ///End of attack

                Destroy(rb.gameObject, projectileLifetime);//destroy trails(clone)
              
            
        }
       
        public void TakeDamage(int damage)
        {
            health -= damage;

        if (audioSource != null && hitSound != null)
            {
            audioSource.PlayOneShot(hitSound);
            }
        if (health <= 0)
             {
                Destroy(gameObject);                
             }
        }
        private void DestroyEnemy()
        {
        Destroy(gameObject);
        } 
        //draw Range
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, sightRange);
        }
    }
