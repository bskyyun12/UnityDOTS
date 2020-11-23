using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public class ProjectilSpawnSystem : ComponentSystem
{
    private float spawnTimer;

    protected override void OnUpdate()
    {
        Entities.
            WithAll<WeaponTag>().
            ForEach((ref Translation translation, ref WeaponData weaponData) =>
            {
                spawnTimer -= Time.DeltaTime;
                if (spawnTimer < 0f)
                {
                    spawnTimer = weaponData.fireRate;

                    Entity spawnedProjectile = EntityManager.Instantiate(weaponData.projectilePrefab);
                    EntityManager.SetComponentData(spawnedProjectile, new Translation { Value = translation.Value });
                    EntityManager.SetComponentData(spawnedProjectile, new MoveData { moveDirection = GetFireDirection(), moveSpeed = GetProjectileSpeed(spawnedProjectile) });
                }
            });
    }

    private float GetProjectileSpeed(Entity spawnedProjectile)
    {
        return EntityManager.GetComponentData<MoveData>(spawnedProjectile).moveSpeed;
    }

    private float3 GetFireDirection()
    {
        Entity playerEntity = GetEntityQuery(ComponentType.ReadOnly<PlayerTag>()).GetSingletonEntity();
        float3 fireDirection = math.normalizesafe(EntityManager.GetComponentData<MoveData>(playerEntity).lookDirection);
        return fireDirection;
    }
}
