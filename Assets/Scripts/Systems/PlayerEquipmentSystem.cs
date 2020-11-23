using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class PlayerEquipmentSystem : SystemBase
{
    protected override void OnCreate()
    {
        //base.OnCreate();
        //Entities.ForEach((in Entity entity, in EquipmentData equipmentData) =>
        //{
        //    SetComponent(equipmentData.weaponPrefab, new Parent { Value = equipmentData.weaponSocket });

        //}).Run();
    }

    protected override void OnUpdate()
    {

    }
}
