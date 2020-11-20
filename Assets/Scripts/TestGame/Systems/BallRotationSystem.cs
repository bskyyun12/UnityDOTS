using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class BallRotationSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;

        Entities.
            WithAll<SpinnerTag>().
            ForEach((ref Rotation rotation, in MoveData moveData) =>
            {
                quaternion normalizedRotation = math.normalizesafe(rotation.Value);
                float angleInRadian = moveData.turnSpeed;
                quaternion angleToRotate = quaternion.AxisAngle(math.up(), angleInRadian * deltaTime);

                rotation.Value = math.mul(normalizedRotation, angleToRotate);

            }).ScheduleParallel();
    }
}
