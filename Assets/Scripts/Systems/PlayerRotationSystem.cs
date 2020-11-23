using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[AlwaysSynchronizeSystem]
public class PlayerRotationSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float3 mouseWorldPosition = float3.zero;

        Entities.
            WithAll<PlayerTag>().
            ForEach((ref Rotation rotation, ref MoveData moveData, in Translation translation) =>
            {
                Plane plane = new Plane(Vector3.up, 0);

                float distance;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (plane.Raycast(ray, out distance))
                {
                    mouseWorldPosition = ray.GetPoint(distance);
                }

                moveData.lookDirection = mouseWorldPosition - translation.Value;
                Debug.DrawRay(translation.Value, moveData.lookDirection);

                quaternion targetRotation = quaternion.LookRotationSafe(moveData.lookDirection, math.up());
                rotation.Value = math.slerp(rotation.Value, targetRotation, moveData.turnSpeed);

            }).Run();
    }
}