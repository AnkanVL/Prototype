using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MonsterAI : MonoBehaviour
{
    NavMeshAgent agent; 
    public Transform target; //Referens till agentens mål
    PeekPoint[] peekPoints;
    private float stalkDistance = 12f;
    float repathTimer;
    PeekPoint currentPeekPoint;
    bool reachedHideSpot = false;
    private float hideTimer = 0;
    public float attackDistance = 2f;
    private bool coroutineRunning;
    float hideDecisionTimer;

    float hideChance = 0.3f;
    //float timer = 0;
    public FieldOfView fieldOfView; //Referens till FW-scriptet
    public static bool isHiding;

    private float distanceToPlayer;

    public enum STATE {STALK, ATTACK, FLEE, HIDE, REPOSITION} //Enumeration som innehåller alla states
    public STATE state;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        hideTimer = 0;
        peekPoints = FindObjectsByType<PeekPoint>(FindObjectsSortMode.None);
        state = STATE.STALK;
    }

    // Update is called once per frame
    void Update()
    {
        distanceToPlayer = Vector3.Distance(transform.position,target.position);
        
        if(distanceToPlayer <= attackDistance)
        {
            //state = STATE.ATTACK;
            Debug.Log("Player dead");
        }
        if (fieldOfView.canSeeMonster && distanceToPlayer < 7)
        {
            state = STATE.FLEE;
        }

        if(fieldOfView.canSeeMonster)
        {
            agent.speed = 3f;
        }
        else
        {
            agent.speed = 9f;
        }

        switch (state)  //FSM   
        { 
            case STATE.STALK:
                ChasePlayer();
                //Debug.Log("Chasing player");
                break;
            case STATE.ATTACK:
                //Attack();
                break;
            case STATE.FLEE:
                Flee();
                //Debug.Log("Fleeing from player");
                break;
            case STATE.HIDE:
                Hide();
                //Debug.Log("Fleeing from player");
                break;
            case STATE.REPOSITION:
                //HIDE();
                //Debug.Log("Fleeing from player");
                break;
        }

        //Debug.Log(fieldOfView.canSeeMonster);
        Debug.Log(state);
    }

    void ChasePlayer()
    {
        agent.isStopped = false;
        
        //Vector3 behindPosition = target.position - target.forward;
        //agent.destination = target.position;
        //agent.SetDestination(behindPosition);

        /*if(Random.value < hideChance * Time.deltaTime)
        {
            EnterHideState();
        }
        */

        repathTimer -= Time.deltaTime;

        if(repathTimer <= 0)
        {
        repathTimer = 1f;

        Vector3 bestPosition = FindHiddenPosition();

        agent.SetDestination(bestPosition);
        }

        hideDecisionTimer -= Time.deltaTime;

        if(hideDecisionTimer <= 0)
        {
            hideDecisionTimer = 5f;

            if(Random.value < hideChance)
            {
                EnterHideState();
            }
        }

        if (fieldOfView.canSeeMonster && distanceToPlayer < 7)
        {
            state = STATE.FLEE;
        }

        if(distanceToPlayer <= attackDistance)
        {
            //state = STATE.ATTACK;
            Debug.Log("Player dead");
        }
        
    }
    
    void EnterHideState()
    {
        
        currentPeekPoint = GetRandomPeekPoint();

        if(currentPeekPoint == null)
            return;

        reachedHideSpot = false;
        agent.isStopped = false;

        agent.SetDestination(currentPeekPoint.transform.position);

        state = STATE.HIDE;
    }

    void Hide()
    {
        if(!reachedHideSpot)
        {
            if(agent.remainingDistance <= 0.5f)
            {
                reachedHideSpot = true;
                agent.isStopped = true;
            }

            return;
        }

        //Debug.Log(hideTimer);
    
        //switch to flee-state if player can see monster
        if(fieldOfView.canSeeMonster && distanceToPlayer < 10)
        {
            state = STATE.FLEE;
            reachedHideSpot = false;
            //agent.isStopped = false;
        }

        /*
        if(distanceToPlayer <= attackDistance)
        {
            state = STATE.ATTACK;
        }
        */
        
        hideTimer += Time.deltaTime;

        if(hideTimer >= 6)
        {
            state = STATE.STALK;
            hideTimer = 0; 
        }

    }

    PeekPoint GetRandomPeekPoint()
{
    return peekPoints[Random.Range(0, peekPoints.Length)];
}

    void Reposition()
    {
        //i dunno if this's gonna get used
    }

    void Attack()
    {
        Debug.Log("Player dead");
        //Kill player
        //Play death animation & sound. All of that stuff ya know?
    }

    void Flee()
    {
        agent.isStopped = false;
        Vector3 fleeDirection = (transform.position - target.position).normalized;
        Vector3 fleePosition = transform.position + fleeDirection * 10f;

        agent.SetDestination(fleePosition); 
        
        if(!coroutineRunning)
        {
            StartCoroutine(StartStalkingAgain());
        }
        
    }

    IEnumerator StartStalkingAgain()
    {
        coroutineRunning = true;
        yield return new WaitForSeconds(5);
        state = STATE.STALK;
        coroutineRunning = false;

    }

    Vector3 FindHiddenPosition()
{
    Vector3 bestPosition = transform.position;

    float bestScore = -999f;

    for(int i = 0; i < 30; i++)
    {
        // Random position around player
        Vector3 randomOffset =
            Random.insideUnitSphere * stalkDistance;

        randomOffset.y = 0;

        Vector3 candidate =
            target.position + randomOffset;

        // Must be on NavMesh
        if(NavMesh.SamplePosition(
            candidate,
            out NavMeshHit hit,
            3f,
            NavMesh.AllAreas))
        {
            candidate = hit.position;

            float score = 0;

            // Behind player = VERY GOOD
            Vector3 dir =
                (candidate - target.position).normalized;

            float dot =
                Vector3.Dot(target.forward, dir);

            if(dot < -0.5f)
            {
                score += 200f;
            }

            // Hidden from player = VERY GOOD
            if(!fieldOfView.CanSeePoint(candidate))
            {
                score += 300f;
            }

            // Close = GOOD
            float dist =
                Vector3.Distance(candidate, target.position);

            score -= dist * 2;

            if(score > bestScore)
            {
                bestScore = score;
                bestPosition = candidate;
            }
        }
    }

    return bestPosition;
}
}
