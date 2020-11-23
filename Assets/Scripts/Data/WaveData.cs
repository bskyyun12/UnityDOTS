using Unity.Entities;

[GenerateAuthoringComponent]
public struct WaveData : IComponentData
{
    public Entity prefabToSpawn;
}