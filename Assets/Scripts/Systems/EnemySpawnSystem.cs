using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public class EnemySpawnSystem : ComponentSystem
{
    private float spawnTimer;
    private int numOfSpawnedEntity;
    private int totalAmountToSpawn;
    private Random random;

    protected override void OnStartRunning()
    {
        base.OnStartRunning();
        random = new Random(55);

        Entities.ForEach((ref WaveData waveData) =>
        {
            totalAmountToSpawn = GetTotalAmountToSpawn(in waveData);
        });
    }

    protected override void OnUpdate()
    {
        Entities.ForEach((ref WaveData waveData) =>
        {
            spawnTimer -= Time.DeltaTime;
            if (spawnTimer < 0f)
            {
                spawnTimer = waveData.spawnDelay;

                SpawnEnemy(waveData);

                numOfSpawnedEntity++;

                //UnityEngine.Debug.Log($"Enemy Spawned! totalAmountToSpawn: {totalAmountToSpawn}");
                if (numOfSpawnedEntity == totalAmountToSpawn)
                {
                    UnityEngine.Debug.Log($"Wave {waveData.currentWave} clear!");
                    numOfSpawnedEntity = 0;
                    waveData.currentWave++;
                    totalAmountToSpawn = GetTotalAmountToSpawn(in waveData);
                }
            }
        });
    }

    private void SpawnEnemy(in WaveData waveData)
    {
        Entity spawnedEntity = EntityManager.Instantiate(waveData.prefabToSpawn);

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

    private int GetTotalAmountToSpawn(in WaveData waveData)
    {
        return (int)math.round(waveData.baseSpawnAmount * waveData.spawnAmountMultiplierPerWave * (waveData.currentWave + 1));
    }
}
