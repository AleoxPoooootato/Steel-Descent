using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIScript : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;
    
    public Vector3 walkPoint;
    public bool walkPointSet;
    public float walkPointRange;

    public float timeBetweenAttacks;
    public bool alreadyAttacked;

    public float sightRange, attackRange;
    public float attackRangeLong;
    public float attackRangeShort;
    public bool playerInSightRange, playerInAttackRange;
    public float waitBetweenPatrols;
    public float RandomMin = 1;
    public float Distance;
    public Quaternion towardsTarget;
    public Vector3 diff;
    public Vector3 you;
    public Vector3 me;

    public GameObject enemyBullet;

    private void Awake()
    {
        player = GameObject.Find("FPSPlayer").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Patroling(){
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet){
            agent.SetDestination(walkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        distanceToWalkPoint.Set(distanceToWalkPoint.x, 0, distanceToWalkPoint.z);

        Distance = distanceToWalkPoint.magnitude;

        if (Distance < 0.5){
            Invoke(nameof(walkPointReset), waitBetweenPatrols);
        }
    }

    private void walkPointReset(){
        walkPointSet = false;
    }

    private void SearchWalkPoint(){
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX + (RandomMin * randomX / Mathf.Abs(randomX)), transform.position.y, transform.position.z + randomZ + (RandomMin * randomZ / Mathf.Abs(randomZ)));

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround)){
            walkPointSet = true;
        }
    }

    private void ChasePlayer(){
        agent.SetDestination(player.position);
    }

    private void AttackPlayer(){
        agent.SetDestination(transform.position);

        transform.LookAt(player);
        
        me = this.transform.position;
        you = player.position;
        diff = player.position - this.transform.position;

        if(!alreadyAttacked){

            Debug.Log("shot a thing");
            Attack();

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack(){
        alreadyAttacked = false;
    }

    void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange){
            Patroling();
            attackRange = attackRangeShort;
        }
        if (playerInSightRange && !playerInAttackRange){
            ChasePlayer();
            attackRange = attackRangeShort;
        }
        if (playerInSightRange && playerInAttackRange){
            AttackPlayer();
            attackRange = attackRangeLong;
        }
    }
    void Attack(){
        //fix this, try to get a quaternion between two vector3s
        towardsTarget = Quaternion.LookRotation(player.position - transform.position/*player.transform.position.x - this.transform.position.x, player.transform.position.y - this.transform.position.y, player.transform.position.z - this.transform.position.z*/);//Quaternion.Euler(player.position.x, player.position.y, player.position.z);
        GameObject boolet = Instantiate(enemyBullet, new Vector3(transform.position.x, transform.position.y, transform.position.z), towardsTarget);//Quaternion.identity);
    }
}
