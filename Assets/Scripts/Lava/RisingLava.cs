using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RisingLava : MonoBehaviour
{
    public float riseSpeed = 1f; // Speed at which the lava rises
    public GameObject player; // Reference to the player GameObject
    public string sceneToRestart = "SampleScene";
    public GameObject fireParticleSystemPrefab;
    public float spawnInterval = .05f;
    private float timeSinceLastSpawn = 0f;
    private float waitToSpawn = 5f;
    private bool fireParticlesStarted = false;
    public float fireParticleSystemDuration = 2f;
    public GameObject successScreen;

    private bool lavaPaused = false;

    void Update()
    {
        if (!lavaPaused)
        {
            // Move the lava plane upwards over time
            transform.Translate(Vector3.up * riseSpeed * Time.deltaTime);
        }

        // Check if the lava collides with the player
        if (player != null && transform.position.y >= player.transform.position.y)
        {
            // Restart the scene
            SceneManager.LoadScene(sceneToRestart);
        }

        // Check if it's time to spawn a fire particle system
        if (!fireParticlesStarted)
        {
            waitToSpawn -= Time.deltaTime;
            if (waitToSpawn <= 0f)
            {
                fireParticlesStarted = true;
            }
        }

        if (fireParticlesStarted)
        {
            timeSinceLastSpawn += Time.deltaTime;
            if (timeSinceLastSpawn >= spawnInterval)
            {
                SpawnFireParticleSystem();
                timeSinceLastSpawn = 0f; // Reset the timer
            }
        }
    }

    void SpawnFireParticleSystem()
    {
        // Instantiate the fire particle system at a random position on the lava plane
        Vector3 randomPosition = new Vector3(Random.Range(-40f, 40f), transform.position.y, Random.Range(-40f, 40f));
        GameObject fireParticleSystem = Instantiate(fireParticleSystemPrefab, randomPosition, Quaternion.identity);

        // Start the timer to destroy the fire particle system
        StartCoroutine(DestroyFireParticleSystem(fireParticleSystem));
    }

    IEnumerator DestroyFireParticleSystem(GameObject fireParticleSystem)
    {
        // Wait for the specified duration before destroying the particle system
        yield return new WaitForSeconds(fireParticleSystemDuration);

        // Check if the particle system still exists (it might have been destroyed by other means)
        if (fireParticleSystem != null)
        {
            // Destroy the fire particle system
            Destroy(fireParticleSystem);
        }
    }

    // Add this method to pause the lava
    public void PauseLava()
    {
        lavaPaused = true;
    }

    // Add this method to resume the lava
    public void ResumeLava()
    {
        lavaPaused = false;
    }
}




