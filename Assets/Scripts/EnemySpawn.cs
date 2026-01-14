using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public Transform player;
    public float spawnDistance = 50f;
    public float spawnInterval = 5f;
    public GameObject enemyPrefab;
    private float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnEnemy();
            timer = 0f;
        }
    }
    void SpawnEnemy()
    {
        Vector3 spawnDirection = Random.onUnitSphere;
        spawnDirection.y = 0; // Keep on the horizontal plane
        spawnDirection.Normalize();
        Vector3 spawnPosition = player.position + spawnDirection * spawnDistance;
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }



}
