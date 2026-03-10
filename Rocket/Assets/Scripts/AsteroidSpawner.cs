using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    [Header("Targeting")]
    [SerializeField] Transform playerTransform; // Drag Rocket here
    [SerializeField] float spawnDistanceAhead = 20f; // Distance to the right of player

    [Header("Settings")]
    public GameObject asteroidPrefab;
    public float spawnRate = 2f;
    public float spawnRangeY = 8f; // How much higher/lower than the rocket they can spawn
    public float asteroidSpeed = 5f;

    float nextSpawnTime;

    void Update()
    {
        if (playerTransform == null) return;

        if (Time.time >= nextSpawnTime)
        {
            SpawnAsteroid();
            nextSpawnTime = Time.time + spawnRate;
        }
    }

    void SpawnAsteroid()
    {
        // Calculate position: Rocket's X + distance, Rocket's Y + random offset
        float randomYOffset = Random.Range(-spawnRangeY, spawnRangeY);
        Vector3 spawnPos = new Vector3(
            playerTransform.position.x + spawnDistanceAhead,
            playerTransform.position.y + randomYOffset,
            0
        );

        GameObject asteroid = Instantiate(asteroidPrefab, spawnPos, Quaternion.identity);

        Rigidbody rb = asteroid.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.left * asteroidSpeed;
            rb.useGravity = false;
        }
    }
}