using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Jobs;

//[AlwaysSynchronizeSystem]
public class WaveSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float elapsedTime = (float)Time.ElapsedTime;

        Entities.ForEach((ref Translation translation, in MoveSpeedData moveSpeed, in WaveData waveData) =>
        {
            float zPosition = waveData.amplitude * math.sin(elapsedTime * moveSpeed.Value
                    + translation.Value.x * waveData.xOffset + translation.Value.y * waveData.yOffset);
            translation.Value = new float3(translation.Value.x, translation.Value.y, zPosition);
        //}).Schedule();    // run the job in regular way
        }).ScheduleParallel();  // run the job in parallel
        //}).Run(); // run the job  // run the job only in the main thread -> need to add [AlwaysSynchronizeSystem] attribute above the class
    }
}
