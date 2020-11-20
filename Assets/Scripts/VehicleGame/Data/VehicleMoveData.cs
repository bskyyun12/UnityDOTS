using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct VehicleMoveData : IComponentData
{
    public int throttle;
    public int steeringThrow;

    public float mass;
    public float maxDrivingForce;
    public float minTurningRadius;
    public float dragCoefficient;
    public float rollingResistanceCoefficient;
    public float maxSideFriction;

    public float3 velocity;
    public float3 acceleration;
}
