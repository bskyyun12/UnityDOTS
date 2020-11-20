using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class VehicleSteeringSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;

        Entities.
            WithAll<VehicleTag>().
            ForEach((ref VehicleMoveData moveData, ref Rotation rotation) =>
            {
                float3 forwardVector = math.forward(rotation.Value);

                float deltaX = math.dot(forwardVector, moveData.velocity) * deltaTime * moveData.steeringThrow;
                float deltaAngle = deltaX / moveData.minTurningRadius;

                quaternion rotationDelta = quaternion.AxisAngle(math.up(), deltaAngle);
                moveData.velocity = math.rotate(rotationDelta, moveData.velocity);

                rotation.Value = math.mul(rotation.Value, rotationDelta);

            }).Schedule();
    }
}
