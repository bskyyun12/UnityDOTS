using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateAfter(typeof(EndFramePhysicsSystem))]
public class ProjectileCollisionSystem : JobComponentSystem
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

    public struct ProjectileCollisionSystemJob : ITriggerEventsJob
    {
        [ReadOnly] public ComponentDataFromEntity<ProjectileTag> projectileGroup;
        [ReadOnly] public ComponentDataFromEntity<EnemyTag> enemyGroup;

        public EntityCommandBuffer entityCommandBuffer;

        public void Execute(TriggerEvent triggerEvent)
        {
            Entity entityA = triggerEvent.EntityA;
            Entity entityB = triggerEvent.EntityB;

            bool isEntityAProjectile = projectileGroup.HasComponent(entityA);
            bool isEntityBProjectile = projectileGroup.HasComponent(entityB);
            bool isEntityAEnemy = enemyGroup.HasComponent(entityA);
            bool isEntityBEnemy = enemyGroup.HasComponent(entityB);

            if (isEntityAProjectile && isEntityBEnemy)
            {
                UnityEngine.Debug.Log($"ProjectileA: {entityA} collides with EnemyB: {entityB}");
                entityCommandBuffer.DestroyEntity(entityA);
                entityCommandBuffer.DestroyEntity(entityB);
            }

            if (isEntityAEnemy && isEntityBProjectile)
            {
                UnityEngine.Debug.Log($"EnemyA: {entityA} collides with ProjectileB: {entityB}");
                entityCommandBuffer.DestroyEntity(entityA);
                entityCommandBuffer.DestroyEntity(entityB);
            }
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new ProjectileCollisionSystemJob();
        job.projectileGroup = GetComponentDataFromEntity<ProjectileTag>(true);
        job.enemyGroup = GetComponentDataFromEntity<EnemyTag>(true);
        job.entityCommandBuffer = commandBufferSystem.CreateCommandBuffer();

        JobHandle jobHandle = job.Schedule(stepPhysicsWorld.Simulation, ref buildPhysicsWorld.PhysicsWorld, inputDeps);

        //jobHandle.Complete();
        commandBufferSystem.AddJobHandleForProducer(jobHandle);

        return jobHandle;
    }
}
