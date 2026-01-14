using Unity.VisualScripting;
using UnityEngine;

public class PlayerAi : MonoBehaviour
{
    public float health;
    public LayerMask whatIsGround;

    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;
    public Transform attackPoint; // Add this at the top with your other public fields


    public float sightRange, attackRange;
    public bool enemyInSightRange, enemyInAttackRange;

    private Transform nearestEnemy;

    private void Update()
    {
        FindNearestEnemy();

        if (nearestEnemy != null)
        {
            float distance = Vector3.Distance(transform.position, nearestEnemy.position);
            enemyInSightRange = distance <= sightRange;
            enemyInAttackRange = distance <= attackRange;

            // Look at the enemy's center (adjust y if needed)
            Vector3 lookTarget = nearestEnemy.position;
            lookTarget.y = attackPoint.position.y; // Only rotate on Y axis
            attackPoint.LookAt(lookTarget);
            transform.LookAt(lookTarget);

            // Debug: show which enemy is targeted
            Debug.DrawLine(transform.position, nearestEnemy.position, Color.red);

            // Shoot if in attack range
            if (enemyInAttackRange && enemyInSightRange)
            {
                AttackEnemy();
            }
        }
        else
        {
            Patroling();
        }
    }

    private void FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("EnemyModel");
        float minDistance = Mathf.Infinity;
        Transform closest = null;

        foreach (GameObject enemy in enemies)
        {
            float dist = Vector3.Distance(transform.position, enemy.transform.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                closest = enemy.transform;
            }
        }
        nearestEnemy = closest;

        // Debug: log enemy found
        if (nearestEnemy != null)
            Debug.Log("Nearest enemy: " + nearestEnemy.name);
        else
            Debug.Log("No enemies found with tag 'EnemyModel'");
    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
        {
            // No movement, just idle or patrol logic if needed
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }

    private void AttackEnemy()
    {
        if (!alreadyAttacked)
        {
            if (projectile == null)
            {
                Debug.LogError("Projectile prefab not assigned!");
                return;
            }
            if (attackPoint == null)
            {
                Debug.LogError("AttackPoint not assigned!");
                return;
            }

            Rigidbody rb = Instantiate(projectile, attackPoint.position, attackPoint.rotation).GetComponent<Rigidbody>();
            rb.AddForce(attackPoint.forward * 32f, ForceMode.Impulse);
            //rb.AddForce(attackPoint.up * 8f, ForceMode.Impulse);

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0) Invoke(nameof(DestroyEnemy), 0.5f);
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }
}
