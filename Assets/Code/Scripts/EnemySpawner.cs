using Fusion;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : NetworkBehaviour
{
    [Networked] public NetworkBool waveStart { get; set; }
    
    // Variáveis transformadas em [Networked] para sobreviverem a quedas de conexão
    // e trocas de autoridade entre os jogadores
    [Networked] public int currentWave { get; set; }
    [Networked] public int enemiesAlive { get; set; }

    [System.Serializable]
    public class WaveContent
    {
        [SerializeField][NonReorderable] NetworkPrefabRef[] enemySpawn;

        public NetworkPrefabRef[] GetEnemySpawnList()
        {
            return enemySpawn;
        }
    }

    [SerializeField] private WaveContent[] waves;
    [SerializeField] private float spawnRange = 10f; // Corrigido o typo

    public override void FixedUpdateNetwork()
    {
        if (!HasStateAuthority) return;

        // Se o spawner foi ativado e não há inimigos vivos, 
        // ele avança para a próxima onda automaticamente.
        // Isso resolve a onda 0 e todas as subsequentes de forma limpa.
        if (waveStart && enemiesAlive == 0)
        {
            if (currentWave < waves.Length)
            {
                SpawnWave();
                currentWave++;
            }
            else
            {
                // Todas as ondas terminaram
                Runner.Despawn(Object);
            }
        }
    }

    private void SpawnWave()
    {
        NetworkPrefabRef[] currentWavePrefabs = waves[currentWave].GetEnemySpawnList();
        
        for (int i = 0; i < currentWavePrefabs.Length; i++)
        {
            NetworkObject newSpawn = Runner.Spawn(
                currentWavePrefabs[i], 
                FindSpawnLoc(), 
                Quaternion.identity
            );

            EnemyController enemy = newSpawn.GetComponent<EnemyController>();
            
            if (enemy != null)
            {
                enemy.SetSpawner(this);
                enemiesAlive++; // Incrementa a contagem de forma sincronizada na rede
            }
        }
        
        Debug.Log($"Onda {currentWave} iniciada. Inimigos: {enemiesAlive}");
    }

    // RPC para garantir que qualquer jogador que mate o inimigo consiga avisar
    // a Autoridade do Spawner para diminuir a contagem.
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void Rpc_ReportEnemyDeath()
    {
        enemiesAlive--;
        
        // Trava de segurança extra
        if (enemiesAlive < 0) enemiesAlive = 0; 
    }

    private Vector3 FindSpawnLoc()
    {
        for (int i = 0; i < 20; i++)
        {
            float xLoc = Random.Range(-spawnRange, spawnRange) + transform.position.x;
            float zLoc = Random.Range(-spawnRange, spawnRange) + transform.position.z;

            Vector3 randomPos = new Vector3(xLoc, transform.position.y, zLoc);
            NavMeshHit hit;

            if (NavMesh.SamplePosition(randomPos, out hit, 5f, NavMesh.AllAreas))
            {
                return hit.position;
            }
        }

        return transform.position; // Fallback caso não ache posição
    }
}