using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class ProjectileMovementSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;

        Entities.
            WithAll<ProjectileTag>().
            ForEach((ref Translation translation, in MoveData moveData) =>
            {
                float3 normalizedDirection = math.normalizesafe(moveData.moveDirection);
                normalizedDirection.y = 0f;
                translation.Value += normalizedDirection * moveData.moveSpeed * deltaTime;

            }).Run();
    }
}
