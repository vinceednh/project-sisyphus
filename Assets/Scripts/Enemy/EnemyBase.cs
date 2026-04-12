using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public float speed = 3.0f;
    public float detectionRange = 10f;
    public float loseAggroRange = 15f;
    public float stopDistance = 1.5f;
    
    protected Transform player;
    protected UnityEngine.AI.NavMeshAgent agent;
    protected bool playerSpotted = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.speed = speed;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.position);
            if (distance <= detectionRange && CanSeePlayer())
            {
                playerSpotted = true;
            }
            
            if (playerSpotted)
            {
                if (distance > stopDistance)
                {
                    agent.SetDestination(player.position);
                }
                else
                {
                    agent.ResetPath();
                }
            }
        }
    }

    protected bool CanSeePlayer()
    {
        Vector3 direction = player.position - transform.position;
        if (Physics.Raycast(transform.position, direction, out RaycastHit hit))
        {
            return hit.transform.root.CompareTag("Player");
        }
        return false;
    }
}
