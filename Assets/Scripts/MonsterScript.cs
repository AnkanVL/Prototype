using UnityEngine;
using UnityEngine.AI;

public class MonsterScript : MonoBehaviour
{
    public Transform player;
    public float InactiveSpeed = 2f;
    public float chaseSpeed = 6f;
    public float viewDistance = 15f;
    public float viewAngle = 75f;
    public float smellDistance = 4f; // Radien där monstret "luktar" dig genom väggar

    [Header("Wander Settings")]
    public float wanderRadius = 10f;
    public float wanderTimer = 5f;
    private float timer;

    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        timer = wanderTimer;

        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        ChaseFunction();
    }

    void ChaseFunction()
    {
        // Kontrollera om monstret antingen ser ELLER luktar spelaren
        if (CanSeePlayer() || CanSmellPlayer())
        {
            // Jaga spelaren
            agent.speed = chaseSpeed;
            agent.SetDestination(player.position);
            timer = wanderTimer;
        }
        else
        {
            // Vandra slumpmässigt
            agent.speed = InactiveSpeed;
            timer += Time.deltaTime;

            if (timer >= wanderTimer || (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending))
            {
                Vector3 newPos = GetRandomNavMeshPoint(transform.position, wanderRadius);
                agent.SetDestination(newPos);
                timer = 0;
            }
        }
    }

    bool CanSeePlayer()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        float distance = directionToPlayer.magnitude;

        if (distance < viewDistance && Vector3.Angle(transform.forward, directionToPlayer) < viewAngle / 2f)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position + Vector3.up, directionToPlayer.normalized, out hit, viewDistance))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    return true;
                }
            }
        }
        return false;
    }

    // Ny logik för att känna lukt
    bool CanSmellPlayer()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        // Returnerar sant om spelaren är tillräckligt nära, oavsett vinkel eller hinder
        return distance < smellDistance;
    }

    public static Vector3 GetRandomNavMeshPoint(Vector3 center, float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += center;
        NavMeshHit hit;
        // Hittar närmaste punkt på [NavMesh](https://docs.unity3d.com)
        NavMesh.SamplePosition(randomDirection, out hit, radius, NavMesh.AllAreas);
        return hit.position;
    }

    // Ritar ut luktradien i Scene-vyn så du kan se den när du testar
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, smellDistance);
    }
}
