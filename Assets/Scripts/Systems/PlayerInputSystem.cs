using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

[AlwaysSynchronizeSystem]
public class PlayerInputSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities.
            WithAll<PlayerTag>().
            ForEach((ref MoveData moveData, in InputData inputData) =>
            {
                bool isRightKeyPressed = Input.GetKey(inputData.rightKey);
                bool isLeftKeyPressed = Input.GetKey(inputData.leftKey);
                bool isUpKeyPressed = Input.GetKey(inputData.upKey);
                bool isDownKeyPressed = Input.GetKey(inputData.downKey);

                moveData.moveDirection.x = (isRightKeyPressed) ? 1 : 0;
                moveData.moveDirection.x -= (isLeftKeyPressed) ? 1 : 0;
                moveData.moveDirection.z = (isUpKeyPressed) ? 1 : 0;
                moveData.moveDirection.z -= (isDownKeyPressed) ? 1 : 0;

            }).Run();
    }
}