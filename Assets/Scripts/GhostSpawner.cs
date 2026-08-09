using UnityEngine;

public class GhostSpawner : MonoBehaviour
{
    [SerializeField] private GameObject ghost;
    private float randomX;
    private float randomZ;
    private Vector3 randomPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for(int numberOfGhosts = 0; numberOfGhosts < 10; numberOfGhosts++)
        {
            // SpawnGhost();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Get a random position wihtin bounds and spawn a ghost there
    /// </summary>
    void SpawnGhost()
    {
        randomX = Random.Range(-10f, 10f);
        randomZ = Random.Range(-10f, 10f);
        randomPosition = new Vector3(randomX, 2f, randomZ);
        Instantiate(ghost, randomPosition, ghost.transform.rotation);
    }
}

