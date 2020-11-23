using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public class PlayerEquipmentSpawnSystem : ComponentSystem
{
    Entity spawnedWeapon;

    protected override void OnStartRunning()
    {
        base.OnStartRunning();

        Entities.ForEach((ref Translation translation, ref EquipmentData equipmentData) =>
        {
            // spawn a weapon from the equipmentData
            spawnedWeapon = EntityManager.Instantiate(equipmentData.weaponPrefab);
            EntityManager.AddComponentData(spawnedWeapon, new Parent { Value = equipmentData.weaponSocket });
        });
    }

    protected override void OnUpdate()
    {
        Entities.ForEach((ref Translation translation, ref EquipmentData equipmentData) =>
        {
            // spawn a weapon from the equipmentData
            EntityManager.SetComponentData(spawnedWeapon, new Translation { Value = TryGetWeaponSocketPos(translation, equipmentData) });
        });

    }

    private float3 TryGetWeaponSocketPos(in Translation translation, in EquipmentData equipmentData)
    {
        ComponentDataFromEntity<LocalToWorld> localToWorldGroup = GetComponentDataFromEntity<LocalToWorld>(true);
        if (localToWorldGroup.HasComponent(equipmentData.weaponSocket))
        {
            // if weaponSocket exists as a child of player
            return localToWorldGroup[equipmentData.weaponSocket].Position;
        }
        else
        {
            // return player's position
            return translation.Value;
        }
    }
}
