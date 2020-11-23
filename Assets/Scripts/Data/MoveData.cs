using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct MoveData : IComponentData
{
    public float3 moveDirection;
    public float3 lookDirection;
    public float moveSpeed;
    public float moveSpeedRandomDeviation;
    public float turnSpeed;
    public float turnSpeedRandomDeviation;
}
