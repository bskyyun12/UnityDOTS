using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[AlwaysSynchronizeSystem]
public class PlayerInputSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities.
            WithAll<PlayerTag>().
            ForEach((ref MoveData moveData, in InputData inputData) => {

            bool isRightKeyPressed = Input.GetKey(inputData.rightKey);
            bool isLeftKeyPressed = Input.GetKey(inputData.leftKey);
            bool isUpKeyPressed = Input.GetKey(inputData.upKey);
            bool isDownKeyPressed = Input.GetKey(inputData.downKey);

            moveData.direction.x = (isRightKeyPressed) ? 1 : 0;
            moveData.direction.x -= (isLeftKeyPressed) ? 1 : 0;
            moveData.direction.z = (isUpKeyPressed) ? 1 : 0;
            moveData.direction.z -= (isDownKeyPressed) ? 1 : 0;

        }).Run();
    }
}
