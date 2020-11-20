using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class MoveForwardSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;
        
        Entities.
            WithAny<BallTag, ChaserTag>().
            ForEach((ref Translation translation, in Rotation rotation, in MoveData moveData) => 
            {
                // move toward x
                translation.Value += math.forward(rotation.Value) * moveData.speed * deltaTime;

            }).Schedule();
    }
}
