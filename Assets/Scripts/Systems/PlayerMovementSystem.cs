using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

[AlwaysSynchronizeSystem]
public class PlayerMovementSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;

        Entities.
            WithAll<PlayerTag>().
            ForEach((ref Translation translation, in MoveData moveData) =>
            {
                float3 normalizedDirection = math.normalizesafe(moveData.moveDirection);
                translation.Value += normalizedDirection * moveData.moveSpeed * deltaTime;

            }).Run();
    }
}