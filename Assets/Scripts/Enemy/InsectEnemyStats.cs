using UnityEngine;

[CreateAssetMenu(fileName = "InsectEnemyStats", menuName = "Enemies/Insect Stats")]
public class InsectEnemyStats : ScriptableObject
{
    [Header("Identity")]
    public string enemyName = "Insect";

    [Header("Movement")]
    public float moveSpeed = 4f;
    public float detectionRange = 10f;
    public float loseAggroRange = 15f;
    public float flyHeight = 2.5f;

    [Header("Attack")]
    public float attackRange = 1.8f;
    public float attackDamage = 10f;
    public float attackCooldown = 1.2f;
    public float attackRadius = 1.5f;

    [Header("Health")]
    public int maxHealth = 100;
}

