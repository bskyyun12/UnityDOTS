using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct WaveBufferElementData : IBufferElementData
{
    public WaveInfo wave;
}

[Serializable]
public struct WaveInfo
{
    public int test1;
    public int test2;
}