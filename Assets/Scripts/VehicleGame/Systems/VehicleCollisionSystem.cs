using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;
using UnityEngine;

[UpdateBefore(typeof(VehicleMoveSystem))]
public class VehicleCollisionSystem : JobComponentSystem
{
    private BuildPhysicsWorld buildPhysicsWorld;
    private StepPhysicsWorld stepPhysicsWorld;

    protected override void OnCreate()
    {
        base.OnCreate();
        buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
        stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
    }

    struct VehicleCollisionSystemJob : ICollisionEventsJob
    {
        [ReadOnly] public ComponentDataFromEntity<VehicleTag> vehicleColliderGroup;
        [ReadOnly] public ComponentDataFromEntity<WallTag> wallColliderGroup;

        public ComponentDataFromEntity<Translation> translationGroup;
        public ComponentDataFromEntity<VehicleMoveData> vehicleMoveGroup;

        public PhysicsWorld physicsWorld;

        public void Execute(CollisionEvent collisionEvent)
        {
            Entity entityA = collisionEvent.EntityA;
            Entity entityB = collisionEvent.EntityB;

            bool isEntityAVehicle = vehicleColliderGroup.HasComponent(entityA);
            bool isEntityBVehicle = vehicleColliderGroup.HasComponent(entityB);

            bool isEntityAWall = wallColliderGroup.HasComponent(entityA);
            bool isEntityBWall = wallColliderGroup.HasComponent(entityB);

            if (isEntityAVehicle && isEntityBWall)
            {
                HandleCollision(entityA, entityB, collisionEvent);
                Debug.Log($"A: vehicle");

            }

            if (isEntityAWall && isEntityBVehicle)
            {
                HandleCollision(entityB, entityA, collisionEvent);
                Debug.Log($"B: vehicle");
            }
        }

        private void HandleCollision(Entity vehicleEntity, Entity wallEntity, CollisionEvent collisionEvent)
        {
            VehicleMoveData moveData = vehicleMoveGroup[vehicleEntity];
            Translation vehiclePosition = translationGroup[vehicleEntity];

            float3 v1i = moveData.velocity;
            float m1 = moveData.mass;

            float3 v2i = float3.zero;
            float m2 = 99999f;

            float3 impactPoint = collisionEvent.CalculateDetails(ref physicsWorld).AverageContactPointPosition;

            // this vehicle
            float3 hitDir1 = vehiclePosition.Value - impactPoint;
            hitDir1.y = 0;
            float3 v1f = v1i - ((m2 + m2) / (m1 + m2)) * math.dot((v1i - v2i), hitDir1) / (math.length(hitDir1) * math.length(hitDir1)) * hitDir1;
            moveData.velocity = v1f;

            vehicleMoveGroup[vehicleEntity] = moveData;

            Debug.Log($"old velocity: {v1i}, new velocity: {v1f}");

            // other
            //float3 HitDir2 = impactPoint - vehiclePosition.Value;
            //HitDir2.z = 0;
            //float3 V2f = V2i - ((M1 + M1) / (M1 + M2)) * math.dot((V2i - V1i), HitDir2) / (math.length(HitDir2) * math.length(HitDir2)) * HitDir2;

        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new VehicleCollisionSystemJob();

        job.vehicleColliderGroup = GetComponentDataFromEntity<VehicleTag>(true);
        job.wallColliderGroup = GetComponentDataFromEntity<WallTag>(true);
        job.vehicleMoveGroup = GetComponentDataFromEntity<VehicleMoveData>(false);
        job.translationGroup = GetComponentDataFromEntity<Translation>(false);
        job.physicsWorld = buildPhysicsWorld.PhysicsWorld;

        JobHandle jobHandle = job.Schedule(stepPhysicsWorld.Simulation, ref buildPhysicsWorld.PhysicsWorld, inputDeps);
        jobHandle.Complete();

        return jobHandle;
    }
}
