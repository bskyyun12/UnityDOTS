using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Physics;
using Unity.Physics.Systems;

//[UpdateAfter(typeof(EndFramePhysicsSystem))]
public class BallTriggerSystem : JobComponentSystem
{

    private BuildPhysicsWorld buildPhysicsWorld;
    private StepPhysicsWorld stepPhysicsWorld;

    private EndSimulationEntityCommandBufferSystem commandBufferSystem;

    protected override void OnCreate()
    {
        base.OnCreate();
        buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
        stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
        commandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    //[BurstCompile]
    struct BallTriggerSystemJob : ITriggerEventsJob
    {
        [ReadOnly] public ComponentDataFromEntity<BallTag> allBalls;
        [ReadOnly] public ComponentDataFromEntity<PlayerTag> allPlayers;

        public EntityCommandBuffer entityCommandBuffer;

        public void Execute(TriggerEvent triggerEvent)
        {
            Entity entityA = triggerEvent.EntityA;
            Entity entityB = triggerEvent.EntityB;

            if (allBalls.HasComponent(entityA) && allBalls.HasComponent(entityB))
            {
                return;
            }

            if (allBalls.HasComponent(entityA) && allPlayers.HasComponent(entityB))
            {
                UnityEngine.Debug.Log($"Ball entityA: {entityA} collided with Player entityB: {entityB}");
                entityCommandBuffer.DestroyEntity(entityA);
            }
            else if (allPlayers.HasComponent(entityA) && allBalls.HasComponent(entityB))
            {
                UnityEngine.Debug.Log($"Player entityA: {entityA} collided with Ball entityB: {entityB}");
                entityCommandBuffer.DestroyEntity(entityB);

            }
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        BallTriggerSystemJob job = new BallTriggerSystemJob();
        job.allBalls = GetComponentDataFromEntity<BallTag>(true);
        job.allPlayers = GetComponentDataFromEntity<PlayerTag>(true);
        job.entityCommandBuffer = commandBufferSystem.CreateCommandBuffer();

        JobHandle jobHandle = job.Schedule(stepPhysicsWorld.Simulation, ref buildPhysicsWorld.PhysicsWorld, inputDeps);

        //jobHandle.Complete();
        commandBufferSystem.AddJobHandleForProducer(jobHandle);

        return jobHandle;
    }
}
