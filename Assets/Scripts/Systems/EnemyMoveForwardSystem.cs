using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class EnemyMoveForwardSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;

        Entities.
            WithAll<EnemyTag>().
            ForEach((ref Translation translation, in Rotation rotation, in MoveData moveData) =>
            {
                // move toward x
                translation.Value += math.forward(rotation.Value) * moveData.moveSpeed * deltaTime;

            }).Schedule();
    }
}
