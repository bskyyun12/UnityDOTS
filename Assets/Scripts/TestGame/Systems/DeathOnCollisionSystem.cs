using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Physics;
using Unity.Physics.Systems;

//[UpdateAfter(typeof(EndFramePhysicsSystem))]
public class DeathOnCollisionSystem : JobComponentSystem
{
    private BuildPhysicsWorld buildPhysicsWorld;
    private StepPhysicsWorld stepPhysicsWorld;

    protected override void OnCreate()
    {
        base.OnCreate();
        buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
        stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
    }

    struct DeathOnCollisionSystemJob : ICollisionEventsJob
    {
        [ReadOnly] public ComponentDataFromEntity<BallTag> ballColliderGroup;
        [ReadOnly] public ComponentDataFromEntity<ChaserTag> chaserColliderGroup;

        public ComponentDataFromEntity<HealthData> healthGroup;

        public void Execute(CollisionEvent collisionEvent)
        {
            Entity entityA = collisionEvent.EntityA;
            Entity entityB = collisionEvent.EntityB;

            bool entityAIsChaser = chaserColliderGroup.HasComponent(entityA);
            bool entityBIsChaser = chaserColliderGroup.HasComponent(entityB);

            bool entityAIsBall = ballColliderGroup.HasComponent(entityA);
            bool entityBIsBall = ballColliderGroup.HasComponent(entityB);

            if (entityAIsBall && entityBIsChaser)
            {
                HealthData healthData = healthGroup[entityB];
                healthData.isDead = true;
                healthGroup[entityB] = healthData;
            }

            if (entityAIsChaser && entityBIsBall)
            {
                HealthData healthData = healthGroup[entityA];
                healthData.isDead = true;
                healthGroup[entityA] = healthData;
            }
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new DeathOnCollisionSystemJob();

        job.ballColliderGroup = GetComponentDataFromEntity<BallTag>(true);
        job.chaserColliderGroup = GetComponentDataFromEntity<ChaserTag>(true);
        job.healthGroup = GetComponentDataFromEntity<HealthData>(false);

        JobHandle jobHandle = job.Schedule(stepPhysicsWorld.Simulation, ref buildPhysicsWorld.PhysicsWorld, inputDeps);
        jobHandle.Complete();

        return jobHandle;
    }
}
