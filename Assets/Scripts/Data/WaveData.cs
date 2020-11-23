using Unity.Entities;

[GenerateAuthoringComponent]
public struct WaveData : IComponentData
{
    public Entity prefabToSpawn;
    public int spawnDelay;
    public int baseSpawnAmount;
    public float spawnAmountMultiplierPerWave;
    public int currentWave;
}