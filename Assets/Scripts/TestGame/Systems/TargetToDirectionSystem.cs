using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class TargetToDirectionSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities.
            WithAll<ChaserTag>().
            ForEach((ref Rotation rotation, ref MoveData moveData, in TargetData targetData, in Translation translation) =>
            {
                // Get a specific component from an entity
                ComponentDataFromEntity<Translation> allTranslations = GetComponentDataFromEntity<Translation>(true);
                if (!allTranslations.HasComponent(targetData.targetEntity))
                {
                    return;
                }
                Translation targetPosition = allTranslations[targetData.targetEntity];

                float3 directionToTarget = targetPosition.Value - translation.Value;
                moveData.direction = directionToTarget;

                //FaceDirection(ref rotation, moveData);
            }).Run();
    }
}
