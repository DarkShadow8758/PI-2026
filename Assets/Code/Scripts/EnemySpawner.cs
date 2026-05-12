using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public bool waveStart = false;
    [System.Serializable]
    public class WaveContent
    {
        [SerializeField][NonReorderable] GameObject[] enemySpawn;

        public GameObject[] GetEnemySpawnList()
        {
            return enemySpawn;
        }
    }

    [SerializeField] WaveContent[] waves;
    int currentWave = 0;
    float spawRange = 10;
    public List <GameObject> currentEnemy;

    // Start is called before the first frame update
    void Start()
    {
        if (waveStart) SpawnWave();
    }

    // Update is called once per frame
    void Update()
    {
        if ( waveStart && currentEnemy.Count == 0)
        {
            currentWave++;
            if (currentWave < waves.Length)
            {
                SpawnWave();
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    void SpawnWave()
    {
        for(int i = 0; i < waves[currentWave].GetEnemySpawnList().Length; i++)
        {
            GameObject newSpawn = Instantiate(waves[currentWave].GetEnemySpawnList()[i], FindSpawnLoc(), Quaternion.identity);
            currentEnemy.Add(newSpawn);

            EnemyController enemy = newSpawn.GetComponent<EnemyController>();
            enemy.SetSpawner(this);
        }
    }

    Vector3 FindSpawnLoc()
    {
        Vector3 SpawnPos;

        float xLoc = Random.Range(-spawRange, spawRange) + transform.position.x;
        float yLoc = transform.position.y;
        float zLoc = Random.Range(-spawRange, spawRange) + transform.position.z;

        SpawnPos = new Vector3(xLoc, yLoc, zLoc);

        if (Physics.Raycast(SpawnPos, Vector3.down, 5))
        {
            return SpawnPos;
        }
        else
        {
            return FindSpawnLoc();
        }
    }
}
