using Unity.Entities;

[GenerateAuthoringComponent]
public struct EquipmentData : IComponentData
{
    public Entity weaponPrefab;
    public Entity weaponSocket;
}
