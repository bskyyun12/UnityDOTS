using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class EnemyRotationSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities.
            WithAll<EnemyTag>().
            ForEach((ref Rotation rotation, in MoveData moveData, in TargetData targetData, in Translation translation) =>
            {
                ComponentDataFromEntity<Translation> translationGroup = GetComponentDataFromEntity<Translation>(true);
                if (!translationGroup.HasComponent(targetData.targetEntity))
                {
                    return;
                }

                Translation targetPosition = translationGroup[targetData.targetEntity];

                float3 directionToTarget = targetPosition.Value - translation.Value;

                quaternion targetRotation = quaternion.LookRotationSafe(directionToTarget, math.up());
                rotation.Value = math.slerp(rotation.Value, targetRotation, moveData.turnSpeed);

            }).Schedule();
    }
}
