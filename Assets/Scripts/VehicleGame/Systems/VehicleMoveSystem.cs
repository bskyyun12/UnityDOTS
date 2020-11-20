using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class VehicleMoveSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;

        Entities.
            WithAll<VehicleTag>().
            ForEach((ref Translation translation, ref VehicleMoveData moveData, in Rotation rotation) =>
            {
                float3 forwardVector = math.forward(rotation.Value);
                float3 rightVector = math.cross(forwardVector, math.up());

                float3 force = forwardVector * moveData.maxDrivingForce * moveData.throttle;
                force += GetAirResistance(in moveData);
                force += GetRollingResistance(in moveData);
                force += GetSideFriction(in moveData, in rightVector);

                float3 newAcceleration = force / moveData.mass;
                moveData.velocity += (moveData.acceleration + newAcceleration) * (deltaTime * 0.5f);
                moveData.acceleration = newAcceleration;

                float3 deltaX = (moveData.velocity * deltaTime) + (moveData.acceleration * deltaTime * deltaTime * 0.5f);
                translation.Value += deltaX;

            }).Run();
    }

    private static float3 GetAirResistance(in VehicleMoveData moveData)
    {
        // Air Resistance = Speed^2 * DragCoefficient
        float Speed = math.length(moveData.velocity);
        return -math.normalizesafe(moveData.velocity) * Speed * Speed * moveData.dragCoefficient;
    }

    private static float3 GetRollingResistance(in VehicleMoveData moveData)
    {
        // Rolling Resistance = NormalForce(Mass*Gravity) * DragCoefficient
        float gravity = 9.81f;
        float normalForce = moveData.mass * gravity;   // Normal Force = Mass * Gravity
        return -math.normalizesafe(moveData.velocity) * moveData.rollingResistanceCoefficient * normalForce;
    }
    private static float3 GetSideFriction(in VehicleMoveData moveData, in float3 rightVector)
    {
        float3 DotV = rightVector * math.dot(moveData.velocity, rightVector);
        float3 SideFriction = moveData.mass * -DotV;

        if (math.length(SideFriction) > moveData.maxSideFriction * moveData.mass)
        {
            SideFriction *= moveData.maxSideFriction * moveData.mass / math.length(SideFriction);
        }
        return SideFriction;
    }
}
