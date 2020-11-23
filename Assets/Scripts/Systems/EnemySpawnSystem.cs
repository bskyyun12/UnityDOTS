using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public class EnemySpawnSystem : ComponentSystem
{
    private float spawnTimer;
    private Random random;
    private int currentWave = 0;
    private int numOfSpawnedEntity;
    private Entity prefabToSpawn;

    protected override void OnStartRunning()
    {
        base.OnStartRunning();
        random = new Random(55);

        Entities.ForEach((ref WaveData waveData) => 
        {
            prefabToSpawn = waveData.prefabToSpawn;
        });
    }

    protected override void OnUpdate()
    {
        Entities.ForEach((DynamicBuffer<WaveBufferElementData> waveInfo) =>
        {
            spawnTimer -= Time.DeltaTime;
            if (spawnTimer < 0f)
            {
                if (currentWave > waveInfo.Length - 1)
                {
                    UnityEngine.Debug.Log("There are no more waves");
                    return;
                }

                WaveInfo wave = waveInfo[currentWave].Value;
                spawnTimer = wave.secondsBetweenSpawns;

                for (int i = 0; i < wave.spawnAmountAtATime; i++)
                {
                    SpawnEnemy(prefabToSpawn);
                    ++numOfSpawnedEntity;
                    UnityEngine.Debug.Log($"Spawned an enemy! Wave{currentWave}. {numOfSpawnedEntity}/{wave.totalAmountToSpawn}");
                    if (numOfSpawnedEntity == wave.totalAmountToSpawn)  // move to next wave
                    {
                        UnityEngine.Debug.Log($"Wave{currentWave} clear! nextWave Starts!");
                        numOfSpawnedEntity = 0;
                        ++currentWave;
                    }
                }
            }
        });
    }

    private void SpawnEnemy(in Entity prefabToSpawn)
    {
        Entity spawnedEntity = EntityManager.Instantiate(prefabToSpawn);

        // set position
        EntityManager.SetComponentData(spawnedEntity, new Translation { Value = GetSpawnPosition() });

        // set up moveData
        MoveData moveData = EntityManager.GetComponentData<MoveData>(spawnedEntity);
        EntityManager.SetComponentData(spawnedEntity, new MoveData
        {
            moveSpeed = moveData.moveSpeed + moveData.moveSpeedRandomDeviation * random.NextFloat(-1f, 1f),
            turnSpeed = moveData.turnSpeed + moveData.turnSpeedRandomDeviation * random.NextFloat(-1f, 1f),
            moveSpeedRandomDeviation = moveData.moveSpeedRandomDeviation,
            turnSpeedRandomDeviation = moveData.turnSpeedRandomDeviation
        });

        // set targetEntity to player
        EntityManager.SetComponentData(spawnedEntity, new TargetData
        {
            targetEntity = GetEntityQuery(ComponentType.ReadOnly<PlayerTag>()).GetSingletonEntity()
        });
    }

    private float3 GetSpawnPosition()
    {
        EntityQuery spawnerQuery = GetEntityQuery(ComponentType.ReadOnly<SpawnerTag>()); //EntityQuery playerQuery = GetEntityQuery(typeof(SpawnerTag));
        Entity spawnerEntity = spawnerQuery.GetSingletonEntity(); //Entity playerEntity = playerQuery.ToEntityArray(Allocator.Temp)[0];
        float3 spawnerPosition = EntityManager.GetComponentData<Translation>(spawnerEntity).Value;
        spawnerPosition += new float3(random.NextFloat3() * 20f);
        spawnerPosition.y = 0f;
        return spawnerPosition;
    }
}
