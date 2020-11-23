using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct WaveBufferElementData : IBufferElementData
{
    public WaveInfo Value;
}

[Serializable]
public struct WaveInfo
{
    public float secondsBetweenSpawns;
    public int totalAmountToSpawn;
    public int spawnAmountAtATime;
}