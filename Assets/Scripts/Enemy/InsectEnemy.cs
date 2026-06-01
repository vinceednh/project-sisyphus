using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class InsectEnemy : EnemyBase
{
    public InsectEnemyStats stats;
    private Animator anim;
    private EnemyHealth health;
    private enum State { Idle, Patrol, Flying, Landing, Walking, Attacking, Dead }
    private State state = State.Idle;
    private float attackTimer = 0f;
    private float landTimer = 0f;
    private Vector3 spawnPoint;
    private float patrolWaitTimer = 0f;
    private const float PatrolWaitTime = 2f;
    private const float PatrolRadius = 8f;
    private const float LandDuration = 0.6f;

    private static readonly int HashFlying = Animator.StringToHash("isFlying");
    
    private static readonly int HashWalking = Animator.StringToHash("isWalking");
    private static readonly int HashAttacking = Animator.StringToHash("isAttacking");
    private static readonly int HashDie = Animator.StringToHash("Die");

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        
        if (stats != null)
        {
            speed = stats.moveSpeed;
            detectionRange = stats.detectionRange;
            loseAggroRange = stats.loseAggroRange;
            stopDistance = stats.attackRange;
        }

        base.Start();

        spawnPoint = transform.position;

        agent.stoppingDistance  = stats != null ? stats.attackRadius : 1.5f;
        anim = GetComponent<Animator>();
        health = GetComponent<EnemyHealth>();

        if (health != null && stats != null)
        {
            health.maxHealth = stats.maxHealth;
        }

        agent.updateUpAxis = false;

        EnterPatrol();
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (state == State.Dead)
        {
            return;
        }

        if (health != null && health.health <= 0)
        {
            Die();
            return;
        }

        if (state == State.Attacking)
        {
            agent.isStopped = true;
        }
        else
        {
            agent.isStopped = false;
        }
        
        base.Update();

        float dist = player != null ? Vector3.Distance(transform.position, player.position) : float.MaxValue;

        switch (state)
        {
            case State.Idle: HandleIdle();
                break;

            case State.Patrol: HandlePatrol();
                break;

            case State.Flying: HandleFlying(dist);
                break;

            case State.Landing: HandleLanding();
                break;

            case State.Walking: HandleWalking(dist);
                break;
            
            case State.Attacking: HandleAttacking(dist);
                break;
        }
    }

    private void HandleIdle()
    {
        SetAnimIdle();
        if (playerSpotted)
        {
            EnterFly();
        }
        else
        {
            EnterPatrol();
        }
    }

    private void HandlePatrol()
    {
        if (playerSpotted)
        {
            EnterFly();
            return;
        }

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            patrolWaitTimer += Time.deltaTime;
            anim.SetBool(HashWalking, false);
            if (patrolWaitTimer >= PatrolWaitTime)
            {
                patrolWaitTimer = 0f;
                EnterPatrol();
            }
        } else
        {
            anim.SetBool(HashWalking, true);
        }
    }

    private void HandleFlying(float dist)
    {
        if (!playerSpotted)
        {
            EnterPatrol();
            return;
        }

        float targetHeight = stats != null ? stats.flyHeight : 2.5f;
        agent.baseOffset = Mathf.Lerp(agent.baseOffset, targetHeight, Time.deltaTime * 4f);

        anim.SetBool(HashFlying, true);
        anim.SetBool(HashWalking, false);
        anim.SetBool(HashAttacking, false);

        float attackRange = stats != null ? stats.attackRange : 1.8f;
        if (dist <= attackRange)
        {
            EnterLand();
        }
    }

    private void HandleLanding()
    {
        agent.isStopped = false;
        agent.baseOffset = Mathf.Lerp(agent.baseOffset, 0f, Time.deltaTime * 5f);
        agent.ResetPath();

        anim.SetBool(HashFlying, false);
        anim.SetBool(HashAttacking, false);

        landTimer += Time.deltaTime;

        if (landTimer >= LandDuration)
        {
            landTimer = 0f;
            EnterWalk();
        }
    }

    private void HandleWalking(float dist)
    {
        if (!playerSpotted)
        {
            EnterPatrol();
            return;
         }

        if (player != null)
        {
            agent.SetDestination(player.position);
        }

        anim.SetBool(HashFlying, false);
        anim.SetBool(HashWalking, true);
        anim.SetBool(HashAttacking, false);

        float attackRadius = stats != null ? stats.attackRadius : 1.5f;
        if (dist <= attackRadius)
        {
            EnterAttack();
        }

        float attackRange = stats != null ? stats.attackRange : 2.0f;
        if (dist > attackRange * 2f)
        {
            EnterFly();
        }
    }

    private void HandleAttacking(float dist)
    {
        float attackRange = stats != null ? stats.attackRange : 1.8f;

        if (player != null)
        {
            Vector3 dir = player.position - transform.position;
            dir.y = 0f;
            if (dir != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 8f);
            }
        }

        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0f)
        {
            StartCoroutine(AttackCoroutine());
            attackTimer = stats != null ? stats.attackCooldown : 1.2f;
        }

        if (dist > attackRange * 1.1f)
        {
            EnterWalk();
        }
    }

    private IEnumerator AttackCoroutine()
    {
        anim.SetBool(HashAttacking, false);
        yield return null;
        anim.SetBool(HashAttacking, true);
    }
    
    private void EnterIdle()
    {
        state = State.Idle;
        agent.baseOffset = 0f;
        agent.ResetPath();
        SetAnimIdle();
    }

    private void EnterFly()
    {
        state = State.Flying;
        anim.SetBool(HashFlying, true);
        anim.SetBool(HashAttacking, false);
    }

    private void EnterLand()
    {
        state = State.Landing;
        landTimer = 0f;
        anim.SetBool(HashFlying, false);
        anim.SetBool(HashAttacking, false);
    }

    private void EnterWalk()
    {
        state = State.Walking;
        anim.SetBool(HashFlying, false);
        anim.SetBool(HashWalking, true);
        anim.SetBool(HashAttacking, false);
    }

    private void EnterAttack()
    {
        state = State.Attacking;
        attackTimer = 0f;
        agent.ResetPath();
        anim.SetBool(HashFlying, false);
        anim.SetBool(HashWalking, false);
        anim.SetBool(HashAttacking, true);
    }

    private void Die()
    {
        state = State.Dead;
        agent.isStopped = true;
        agent.enabled = false;
        anim.SetTrigger(HashDie);
        Destroy(gameObject, 2f);
    }

    private void DoAttack()
    {
        float radius = stats != null ? stats.attackRadius : 1.5f;
        float damage = stats != null ? stats.attackDamage : 10f;

        Collider[] hits = Physics.OverlapSphere(transform.position, radius);
        foreach (var col in hits)
        {
            if (col.transform.root.CompareTag("Player"))
            {
                col.transform.root.GetComponent<PlayerHealth>()?.TakeDamage((int)damage);
                break;
            }
        }
    }

    public void OnAttackHit()
    {
        if (state != State.Attacking) return;
        DoAttack();
    }

    private void SetAnimIdle()
    {
        anim.SetBool(HashFlying, false);
        anim.SetBool(HashWalking, false);
        anim.SetBool(HashAttacking, false);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        if (stats != null)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, stats.attackRadius);
        }
    }

    private void EnterPatrol()
    {
        state = State.Patrol;
        patrolWaitTimer = 0f;

        Vector2 random = Random.insideUnitCircle * PatrolRadius;
        Vector3 candidate = spawnPoint + new Vector3(random.x, 0, random.y);

        if (NavMesh.SamplePosition(candidate, out NavMeshHit hit, PatrolRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        } else
        {
            agent.SetDestination(spawnPoint);
        }

        anim.SetBool(HashFlying, false);
        anim.SetBool(HashWalking, true);
        anim.SetBool(HashAttacking, false);
    }
}
