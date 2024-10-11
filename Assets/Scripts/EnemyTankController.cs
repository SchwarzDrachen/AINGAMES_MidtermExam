using UnityEngine;

public class EnemyTankController : MonoBehaviour
{
    // Movement variables
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float rotateSpeed = 10.0f;
    // How "close" to the player is considered as chasing range
    [SerializeField] private float chaseDistance = 20.0f;
    [SerializeField] private float attackDistance = 10.0f;
    // How "far" from the player will the tank run away
    [SerializeField] private float fleeDistance = 20.0f;
    // How "close" from the target waypoint until we choose another waypoint
    [SerializeField] private float waypointDistance = 1.0f;
    // How often the enemy will shoot
    [SerializeField] private float shootRate = 1.0f;
    // The array of transform points in the scene that the agent will move
    // towards to while patrolling
    [SerializeField] private Transform[] waypoints;
    // Reference to the player tank
    [SerializeField] private Transform player;
    [SerializeField] private Transform turret;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private GameObject deathParticle;
    [SerializeField] private GameObject bullet;

    private Health health;

    private void Awake()
    {
        health = GetComponent<Health>();
    }

    private void Update()
    {
       
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
        Instantiate(bullet, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
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
}
