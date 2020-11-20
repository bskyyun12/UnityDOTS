using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class FollowEntity : MonoBehaviour
{
    public Entity entityToFollow;
    public float3 postionOffset = float3.zero;
    public quaternion rotationOffset = quaternion.identity;

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
        transform.position = entityPosition.Value + postionOffset;

        Rotation entityRotation = entityManager.GetComponentData<Rotation>(entityToFollow);
        transform.rotation = entityRotation.Value;//math.mul(entityRotation.Value, rotationOffset);
    }
}
