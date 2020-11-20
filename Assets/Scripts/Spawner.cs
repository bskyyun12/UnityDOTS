using Unity.Entities;
using UnityEngine;
using Unity.Transforms;
using Unity.Rendering;
using Unity.Mathematics;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Mesh unitMesh;
    [SerializeField] private Material unitMaterial;

    [Range(1f, 9999f)]
    [SerializeField] int xSize = 10;
    [Range(1f, 9999f)]
    [SerializeField] int ySize = 10;
    [Range(0.1f, 2f)]
    [SerializeField] float spacing = 1f;

    [SerializeField] private GameObject gameObjectPrefab;
    private Entity entityPrefab;
    private World defaultWorld;
    private EntityManager entityManager;

    // Start is called before the first frame update
    void Start()
    {
        //MakeEntity();

        defaultWorld = World.DefaultGameObjectInjectionWorld;
        entityManager = defaultWorld.EntityManager;

        GameObjectConversionSettings settings = GameObjectConversionSettings.FromWorld(defaultWorld, null);
        entityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(gameObjectPrefab, settings);

        InstantiateEntityGrid(xSize, ySize, spacing);
    }

    private void InstantiateEntity(float3 position)
    {
        Entity myEntity = entityManager.Instantiate(entityPrefab);
        entityManager.SetComponentData(myEntity, new Translation { Value = position });
    }

    private void InstantiateEntityGrid(int dimX, int dimY, float spacing = 1f)
    {
        for (int i = 0; i < dimX; i++)
        {
            for (int j = 0; j < dimY; j++)
            {
                InstantiateEntity(new float3(i * spacing, j * spacing, 0f));
            }
        }
    }

    private void MakeEntity(float3 position)
    {
        // Reference to EntityManager
        EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        #region Conversion workflow
        GameObjectConversionSettings settings = GameObjectConversionSettings.FromWorld(defaultWorld, null);
        entityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(gameObjectPrefab, settings);

        // Create Entity using the EntityArchetype above
        Entity myEntity = entityManager.Instantiate(entityPrefab);
        entityManager.SetComponentData(myEntity, new Translation { Value = position });
        #endregion

        #region Pure ECS
        // Define EntityArchetype
        EntityArchetype entityArchetype = entityManager.CreateArchetype(
            typeof(Translation),
            typeof(Rotation),
            typeof(RenderMesh),
            typeof(RenderBounds),
            typeof(LocalToWorld)
            );

        // Create Entity using the EntityArchetype above
        Entity myPureEntity = entityManager.CreateEntity(entityArchetype);

        // Assign data using AddComponentData and AddSharedComponentData
        entityManager.AddComponentData(myPureEntity, new Translation { Value = new float3(2f, 0f, 4f) });
        entityManager.AddSharedComponentData(myPureEntity, new RenderMesh { mesh = unitMesh, material = unitMaterial });
        #endregion
    }

}
