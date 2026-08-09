using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class GhostSpawner : MonoBehaviour
{
    

    [Header("Positioning")]
    [SerializeField] private float randomX;
    [SerializeField] private float randomY;
    [SerializeField] private float randomZ;
    [SerializeField] private Vector3 randomPosition;


    [Header("Bounds")]
    [SerializeField] private float minXBounds = -9;
    [SerializeField] private float maxXBounds = 9;
    [SerializeField] private float minYBounds = 1;
    [SerializeField] private float maxYBounds = 8;
    [SerializeField] private float minZBounds = -3;
    [SerializeField] private float maxZBounds = 3;
    

    [Header("Ghosts")]
    private List<GameObject> ghosts = new List<GameObject>();
    [SerializeField] private int ghostBatchSize = 5;
    [SerializeField] private float ghostBatchCookTime = 2f;


    [Header("References")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject ghostPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameManager.Instance;
        gameManager.OnMiniGameStart.AddListener(StartSpawningGhosts);
        gameManager.OnMiniGameEnd.AddListener(DeleteAllGhosts);
    }

    void OnDisable()
    {
        gameManager.OnMiniGameStart.RemoveListener(StartSpawningGhosts);
        gameManager.OnMiniGameEnd.RemoveListener(DeleteAllGhosts);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void StartSpawningGhosts()
    {
        StartCoroutine("SpawnGhosts");
    }

    private void DeleteAllGhosts()
    {
        while(ghosts.Count > 0)
        {
            Destroy(ghosts[0]);
            ghosts.Remove(ghosts[0]);
        }
    }

    /// <summary>
    /// Get a random position wihtin bounds and spawn a ghost there
    /// </summary>
    IEnumerator SpawnGhosts()
    {
        yield return new WaitForSeconds(ghostBatchCookTime);

        if(gameManager.PlayerActions.CurrentMode == GameManager.GameMode.Throwing)
        {
            ghosts.Clear();

            for(int numberOfGhostsInBatch = 0; numberOfGhostsInBatch < ghostBatchSize; numberOfGhostsInBatch++)
            {
                randomX = Random.Range(minXBounds, maxXBounds);
                randomY = Random.Range(minYBounds, maxYBounds);
                randomZ = Random.Range(minZBounds, maxZBounds);
                randomPosition = new Vector3(randomX, randomY, randomZ);
                GameObject ghost = Instantiate(ghostPrefab, randomPosition, ghostPrefab.transform.rotation);
                ghosts.Add(ghost);
            }

            StartCoroutine("SpawnGhosts");
        }
    }
}

