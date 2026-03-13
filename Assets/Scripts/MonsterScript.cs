using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MonsterScript : MonoBehaviour
{
    [Header("Core References")]
    public Transform player;

    [Header("Movement Settings")]
    public float inactiveSpeed = 2f;
    public float chaseSpeed = 6f;
    public float stoppingDistance = 1.5f;

    [Header("Detection Settings")]
    public float viewDistance = 15f;
    public float viewAngle = 75f;
    public float smellDistance = 4f;

    [Header("Wander Settings")]
    public float wanderRadius = 10f;
    public float wanderTimer = 5f;

    [Header("Search Settings")]
    public float searchDuration = 8f;
    public float searchSpeedMultiplier = 0.7f;

    [Header("Jumpscare / Game Over Settings")]          // ← CHANGED: No damage, instant horror!
    public float catchRange = 2.5f;                     // How close = instant game over
    public float jumpscareDuration = 1.8f;              // How long the scare lasts before restart
    public string jumpscareTrigger = "Jumpscare";       // Animator trigger name (optional)

    private NavMeshAgent agent;
    private float timer;
    private float searchTimeLeft;
    private Vector3 lastKnownPosition;
    private bool isSearching;
    private bool previouslyDetecting;
    private bool gameOverTriggered = false;

    // Optional: If you have an Animator on the monster
    private Animator monsterAnimator;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent missing on " + gameObject.name);
            return;
        }

        monsterAnimator = GetComponent<Animator>(); // Auto-get if you have one

        agent.stoppingDistance = stoppingDistance;
        timer = wanderTimer;
        searchTimeLeft = 0f;

        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (player == null)
            Debug.LogWarning("Player not found! Tag the player as 'Player'.");
    }

    void Update()
    {
        if (player == null || agent == null || gameOverTriggered) return;

        ChaseFunction();
    }

    void ChaseFunction()
    {
        bool currentlyDetecting = CanSeePlayer() || CanSmellPlayer();

        if (currentlyDetecting)
        {
            lastKnownPosition = player.position;
            isSearching = false;
            searchTimeLeft = 0f;

            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= catchRange)
            {
                // INSTANT GAME OVER + JUMPSCARE
                if (!gameOverTriggered)
                {
                    gameOverTriggered = true;
                    StartCoroutine(TriggerJumpscareAndRestart());
                }
                return;
            }

            // Still chasing
            agent.isStopped = false;
            agent.speed = chaseSpeed;
            agent.SetDestination(player.position);

            timer = wanderTimer;
        }
        else
        {
            agent.isStopped = false;

            if (previouslyDetecting)
            {
                isSearching = true;
                searchTimeLeft = searchDuration;
            }

            if (isSearching && searchTimeLeft > 0)
            {
                agent.speed = chaseSpeed * searchSpeedMultiplier;
                agent.SetDestination(lastKnownPosition);
                searchTimeLeft -= Time.deltaTime;

                if (agent.remainingDistance <= agent.stoppingDistance + 0.5f || searchTimeLeft <= 0)
                {
                    isSearching = false;
                    searchTimeLeft = 0f;
                    timer = 0f;
                }
            }
            else
            {
                agent.speed = inactiveSpeed;
                timer += Time.deltaTime;

                if (timer >= wanderTimer ||
                    (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending))
                {
                    Vector3 newPos = GetRandomNavMeshPoint(transform.position, wanderRadius);
                    agent.SetDestination(newPos);
                    timer = 0f;
                }
            }
        }

        previouslyDetecting = currentlyDetecting;
    }

    bool CanSeePlayer()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        float distance = directionToPlayer.magnitude;

        if (distance > viewDistance) return false;
        if (Vector3.Angle(transform.forward, directionToPlayer) > viewAngle / 2f) return false;

        Vector3 eyePosition = transform.position + Vector3.up * 1.8f;
        Vector3 playerEyePosition = player.position + Vector3.up * 1.5f;
        Vector3 dir = playerEyePosition - eyePosition;

        if (Physics.Raycast(eyePosition, dir.normalized, out RaycastHit hit, distance))
        {
            return hit.collider.CompareTag("Player");
        }

        return false;
    }

    bool CanSmellPlayer()
    {
        return Vector3.Distance(transform.position, player.position) < smellDistance;
    }

    IEnumerator TriggerJumpscareAndRestart()
    {
        agent.isStopped = true;
        agent.velocity = Vector3.zero;

        // Make monster look straight at you for maximum terror
        if (player != null)
            transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));

        // === JUMPSCARE HAPPENS HERE ===
        Debug.Log("<color=red>!!! JUMPSCARE ACTIVATED - GAME OVER !!!</color>");

        // . Play monster jumpscare animation (if you have Animator)
        if (monsterAnimator != null)
            monsterAnimator.SetTrigger(jumpscareTrigger);

        //. (Optional) Play scary sound
         AudioSource audio = GetComponent<AudioSource>();
        

        yield return new WaitForSeconds(jumpscareDuration);

        CheckpointManager.Instance.LoadLastCheckpoint();
    }

    // Improved random point with retry logic
    public static Vector3 GetRandomNavMeshPoint(Vector3 center, float radius)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomDirection = Random.insideUnitSphere * radius;
            randomDirection += center;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomDirection, out hit, radius, NavMesh.AllAreas))
                return hit.position;
        }
        return center;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0f, 0f, 0.3f);
        Gizmos.DrawWireSphere(transform.position, smellDistance);

        Gizmos.color = new Color(1f, 1f, 0f, 0.2f);
        Gizmos.DrawWireSphere(transform.position, viewDistance);

        Gizmos.color = new Color(1f, 0f, 1f, 0.5f);        // ← Purple = JUMPSCARE range
        Gizmos.DrawWireSphere(transform.position, catchRange);

        if (viewDistance > 0)
        {
            Vector3 leftDir = Quaternion.Euler(0, -viewAngle / 2f, 0) * transform.forward * viewDistance;
            Vector3 rightDir = Quaternion.Euler(0, viewAngle / 2f, 0) * transform.forward * viewDistance;

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, transform.position + leftDir);
            Gizmos.DrawLine(transform.position, transform.position + rightDir);
        }
    }

    public void AlertToPosition(Vector3 alertPosition)
    {
        lastKnownPosition = alertPosition;
        isSearching = true;
        searchTimeLeft = searchDuration * 1.5f;
        previouslyDetecting = true;
    }

    public void resetMonster()
    {
        //Reset all AI states
        isSearching = false;
        searchTimeLeft = 0;
        timer = wanderTimer;
        previouslyDetecting = false;
        gameOverTriggered = false;

        // Stop Movement
        if (agent != null)
        {
            agent.isStopped = false;
            agent.ResetPath();
            agent.velocity = Vector3.zero;
            agent.speed = inactiveSpeed;
        }

        Debug.Log("<color=yellow>Monster has been reset to patrol mode</color>");
    }
}

