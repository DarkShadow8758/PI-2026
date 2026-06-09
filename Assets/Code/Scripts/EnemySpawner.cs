using Fusion;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : NetworkBehaviour
{
    [Networked] public NetworkBool waveStart { get; set; }
    [Networked] public int currentWave { get; set; }
    [Networked] public int enemiesAlive { get; set; }
    
    // 🔴 NOVA VARIÁVEL: Controla se a área já foi pacificada
    [Networked] public NetworkBool isCleared { get; set; }

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
    [SerializeField] private float spawnRange = 10f; 

    // 🔴 REFERÊNCIA DA PAREDE
    [Header("Barreira de Progressão")]
    [Tooltip("Coloque aqui o GameObject que bloqueia a pista (com Collider).")]
    [SerializeField] private GameObject progressionWall; 

    public override void FixedUpdateNetwork()
    {
        if (!HasStateAuthority) return;

        // Só tenta spawnar se a wave começou, não tem inimigos e a área ainda não foi limpa
        if (waveStart && enemiesAlive == 0 && !isCleared)
        {
            if (currentWave < waves.Length)
            {
                SpawnWave();
                currentWave++;
            }
            else
            {
                // 🔴 Todas as ondas terminaram! Libera a passagem.
                isCleared = true;
                
                // NOTA: Removi o Runner.Despawn(Object) daqui!
                // Se o objeto despawnar na rede, o script para de rodar para o Jogador 2
                // e a parede dele poderia bugar e ficar presa. Mantemos o Spawner vivo, 
                // mas inativo, para ele segurar a parede desligada.
            }
        }
    }

    // 🔴 NOVO MÉTODO: Render cuida do visual e ativação em TODOS os clientes simultaneamente
    public override void Render()
    {
        if (progressionWall != null)
        {
            // A parede fica ativa apenas se a wave já começou E a área ainda não foi limpa.
            // Isso permite que os jogadores andem livres antes de engatilhar o spawner, 
            // e fiquem trancados durante a batalha.
            bool shouldWallBeActive = waveStart && !isCleared;
            
            if (progressionWall.activeSelf != shouldWallBeActive)
            {
                progressionWall.SetActive(shouldWallBeActive);
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
                enemiesAlive++; 
            }
        }
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void Rpc_ReportEnemyDeath()
    {
        enemiesAlive--;
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

        return transform.position;
    }
}