using UnityEngine;

public class Bullet : MonoBehaviour
{

    public int damage = 1; 

    private void Update()
    {
        Destroy(gameObject, 3f);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("PlayerModel"))
        {
            PlayerAi player = collision.gameObject.GetComponent<PlayerAi>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
        }
        else if (collision.gameObject.CompareTag("EnemyModel"))
        {
            EnemyAi enemy = collision.gameObject.GetComponent<EnemyAi>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
        else if (collision.gameObject.CompareTag("Ground"))
        {
            // Optionally handle ground collision
            Destroy(gameObject);
        }

        Destroy(gameObject);
    }
}
