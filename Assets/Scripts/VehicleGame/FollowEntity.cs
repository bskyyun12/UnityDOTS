using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class FollowEntity : MonoBehaviour
{
    public Entity entityToFollow;

    private EntityManager entityManager;

    private void Awake()
    {
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }

    private void LateUpdate()
    {
        if (entityToFollow == null)
        {
            return;
        }

        Translation entityPosition = entityManager.GetComponentData<Translation>(entityToFollow);
        transform.position = entityPosition.Value;

        Rotation entityRotation = entityManager.GetComponentData<Rotation>(entityToFollow);
        transform.rotation = entityRotation.Value;
    }
}
