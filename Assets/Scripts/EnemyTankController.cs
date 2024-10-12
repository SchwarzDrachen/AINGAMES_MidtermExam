using Panda;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
using UnityEngine.UIElements;

public class EnemyTankController : MonoBehaviour
{
    // Movement variables
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float rotateSpeed = 10.0f;
    // How "close" to the player is considered as chasing range
    [SerializeField] private float chaseDistance = 10.0f;
    [SerializeField] private float attackDistance = 5.0f;
    // How "far" from the player will the tank run away
    [SerializeField] private float fleeDistance = 20.0f;
    // How "close" from the target waypoint until we choose another waypoint
    [SerializeField] private float waypointDistance = 1.0f;
    // How often the enemy will shoot
    [SerializeField] private float shootRate = 20.0f;
    // The array of transform points in the scene hat the agent will move
    // towards to while patrolling
    private float fireCD = 0f;
    [SerializeField] private Transform[] waypoints;
    // Reference to the player tank
    [SerializeField] private Transform player;
    [SerializeField] private Transform agent;
    [SerializeField] private Transform turret;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private GameObject deathParticle;
    [SerializeField] private GameObject bullet;
    private Transform currentTarget;
    private Health health;

    private Transform enemyTurret;

    public float distanceToPlayer;
    private Selector rootNode;
    private Sequence s_Patrol;
    private Inverter in_EnemyNear;
    private ActionNode an_EnemyNear;
    private Inverter in_HPZero;
    private ActionNode an_HPZero;
    private ActionNode an_PatrolMovement;
    private Sequence s_Chase;
    private Inverter in_PlayerFar;
    private ActionNode an_PlayerFar;
    private Inverter in_AttackNear;
    private ActionNode an_AttackNear;
    private ActionNode an_ChaseMovement;
    private Sequence s_Attack;
    private ActionNode an_PlayerWithinRange;
    private ActionNode an_Attack;
   private void Awake()
    {
        health = GetComponent<Health>();
        //  player = GameObject.Find("PlayerTank").transform;
        enemyTurret = gameObject.transform.GetChild(0).transform;
    }

    private void Start(){
       an_EnemyNear = new ActionNode(EnemyNear);
       an_HPZero = new ActionNode(HPZero);
       an_PatrolMovement = new ActionNode(PatrolMovement);

       an_PlayerFar = new ActionNode(PlayerFar);
       an_AttackNear = new ActionNode(AttackNear);
       an_ChaseMovement = new ActionNode(ChaseMovement);

       an_PlayerWithinRange = new ActionNode(PlayerWithinRange);
       an_Attack = new ActionNode(Attack);

       in_EnemyNear = new Inverter(an_EnemyNear);
       in_HPZero = new Inverter(an_HPZero);

       in_PlayerFar = new Inverter(an_PlayerFar);
       in_AttackNear = new Inverter(an_AttackNear);

       List<Node> patrolSequenceNode = new();
       patrolSequenceNode.Add(in_EnemyNear);
       patrolSequenceNode.Add(in_HPZero);
       patrolSequenceNode.Add(an_PatrolMovement);
       s_Patrol = new Sequence(patrolSequenceNode);

       List<Node> chaseSequenceNode = new();
        chaseSequenceNode.Add(in_PlayerFar);
        chaseSequenceNode.Add(in_AttackNear);
        chaseSequenceNode.Add(in_HPZero);
        chaseSequenceNode.Add(an_ChaseMovement);
        s_Chase = new Sequence(chaseSequenceNode);

    List<Node> attackSequenceNode = new();
        attackSequenceNode.Add(an_PlayerWithinRange);
        attackSequenceNode.Add(in_HPZero);
        attackSequenceNode.Add(an_Attack);
        s_Attack = new Sequence(attackSequenceNode);

       List<Node> rootNodeSelector = new();
       rootNodeSelector.Add(s_Patrol);
       rootNodeSelector.Add(s_Chase);
       rootNodeSelector.Add(s_Attack);
       rootNode = new Selector(rootNodeSelector);

    }

    private void Update()
    {
        Debug.Log(currentTarget);
        rootNode.Evaluate();
    }

    public void MoveToTarget(Transform currentTarget)
    {
        // Get the vector pointing towards the direction of the target
        Vector3 targetDirection = currentTarget.position - transform.position;
        // Get the roatation that faces the targetDirection
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        // Rotate the tank to face the targetRotation
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation,
            Time.deltaTime * rotateSpeed);
        // Tank is already rotate, simply move forward
        transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);
    }

    public void ShootBullet()
    {
        
        fireCD += 0.1f;
        if (fireCD >= shootRate)
        {
            Instantiate(bullet, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            fireCD = 0f;
        }
    }

    public void Die()
    {
        Instantiate(deathParticle, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Bullet>() != null)
        {
            health.TakeDamage(1);
        }
    }

    private void SetCurrentTarget(Transform target){
        currentTarget = target;
    }

    private void SetTargetWaypoint(){
        //Randomize a value from the array
        int randomIndex = UnityEngine.Random.Range(0, waypoints.Length);
        //Make sure that the new target is not the same as the previous waypoint
        while(waypoints[randomIndex] == currentTarget){
            //Keep randomizing until currentTarget is new
            randomIndex = UnityEngine.Random.Range(0, waypoints.Length);
        }
        SetCurrentTarget(waypoints[randomIndex]);
    }

   private NodeState EnemyNear(){
        if(Vector3.Distance(agent.position,player.position) <= chaseDistance){
            SetCurrentTarget(player);
            return NodeState.SUCCESS;
        }
        else{
            SetTargetWaypoint();
            return NodeState.FAILURE;
        }
   }
   private NodeState HPZero(){
        //return  health.IsDead ? NodeState.SUCCESS : NodeState.FAILURE;
        if(health.IsDead){
            return NodeState.SUCCESS;
        }
        else{
            return NodeState.FAILURE;
        }
   }
   private NodeState PatrolMovement(){
        float distanceToCurrentTarget = Vector3.Distance(agent.position, currentTarget.position);
        if(distanceToCurrentTarget > waypointDistance){
            MoveToTarget(currentTarget);
        }
        else{
            SetTargetWaypoint();
        }
        return NodeState.SUCCESS;
   }

   private NodeState PlayerFar(){
        if(Vector3.Distance(agent.position,player.position) >chaseDistance){
            return NodeState.SUCCESS;
        }
        else{
            return NodeState.FAILURE;
        }
   }

   private NodeState AttackNear(){
    if(Vector3.Distance(agent.position,player.position) <= attackDistance){
            return NodeState.SUCCESS;
        }
        else{
            return NodeState.FAILURE;
        }
   }

   private NodeState ChaseMovement(){
         float distanceToCurrentTarget = Vector3.Distance(agent.position, currentTarget.position);
        if(distanceToCurrentTarget > waypointDistance){
            MoveToTarget(currentTarget);
        }
        return NodeState.SUCCESS;
   }

   private NodeState PlayerWithinRange()
   {
    if (Vector3.Distance(agent.position, player.position) <= attackDistance)
    {
        return NodeState.SUCCESS;
    }
    else
    {
        return NodeState.FAILURE;
    }
   }

   private NodeState Attack()
   {
    // Get the vector pointing towards the direction of the target
    Vector3 targetDirection = currentTarget.position - transform.position;
    // Get the rotation that faces the targetDirection
    Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
    // Rotate the tank to face the targetRotation
    enemyTurret.transform.rotation = Quaternion.Slerp(enemyTurret.transform.rotation, targetRotation,
        Time.deltaTime * rotateSpeed);

    ShootBullet();

    return NodeState.SUCCESS;
   }

   private NodeState Death()
   {
    Instantiate(deathParticle, transform.position, Quaternion.identity);
    Destroy(this.gameObject);

    Instantiate(powerupPrefab, transform.position, Quaternion.identity);
   }
}
