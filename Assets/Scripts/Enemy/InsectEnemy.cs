using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class InsectEnemy : EnemyBase
{
    public InsectEnemyStats stats;
    private Animator anim;
    private EnemyHealth health;
    private enum State { Idle, Flying, Landing, Walking, Attacking, Dead }
    private State state = State.Idle;
    private float attackTimer = 0f;
    private float landTimer = 0f;
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

        playerSpotted = true;

        agent.stoppingDistance  = stats != null ? stats.attackRadius : 1.5f;
        anim = GetComponent<Animator>();
        health = GetComponent<EnemyHealth>();

        if (health != null && stats != null)
        {
            health.maxHealth = stats.maxHealth;
        }

        agent.updateUpAxis = false;
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
    }

    private void HandleFlying(float dist)
    {
        if (!playerSpotted)
        {
            EnterIdle();
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
            EnterIdle();
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
        DoAttack();
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
}
