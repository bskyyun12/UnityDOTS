using Unity.Entities;

[GenerateAuthoringComponent]
public struct WeaponData : IComponentData
{
    public Entity projectilePrefab;
    public float fireRate;
}
