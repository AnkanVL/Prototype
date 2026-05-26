using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class MonsterAI : MonoBehaviour
{
    NavMeshAgent agent; 
    public Transform target; //Referens till agentens mål
    public GameObject killzone;
    float timer = 0;

    public enum STATE { IDLE, PATROL, CHASE, ATTACK, FLEE} //Enumeration som innehåller alla states
    public STATE state;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        killzone.GetComponentInChildren<BoxCollider>();
        state = STATE.CHASE;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)  //FSM   
        { 
            case STATE.IDLE:
                //Idle();
                break;
            case STATE.PATROL:
                //FollowWaypoints();
                break;
            case STATE.CHASE:
                ChasePlayer();
                Debug.Log("Chasing player");
                break;
            case STATE.ATTACK:
                //Attack();
                break;
            case STATE.FLEE:
                Flee();
                Debug.Log("Fleeing from player");
                break;
        }
    }

    void ChasePlayer()
    {
         agent.destination = target.position;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Respawn")
        {
            state = STATE.FLEE;
        }
    }

    void Attack()
    {
        //GameOver
    }

    void Flee()
    {
        agent.destination -= target.position;
        StartCoroutine(StartChasingAgain());
        
    }

    IEnumerator StartChasingAgain()
    {
        yield return new WaitForSeconds(3);
        state = STATE.CHASE;

    }
}
