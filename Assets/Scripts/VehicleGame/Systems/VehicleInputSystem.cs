using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[AlwaysSynchronizeSystem]
public class VehicleInputSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities.ForEach((ref VehicleMoveData moveData, in VehicleInputData inputData) => {
            bool isRightKeyPressed = Input.GetKey(inputData.rightKey);
            bool isLeftKeyPressed = Input.GetKey(inputData.leftKey);
            bool isUpKeyPressed = Input.GetKey(inputData.upKey);
            bool isDownKeyPressed = Input.GetKey(inputData.downKey);

            moveData.throttle = isUpKeyPressed ? 1 : 0;
            moveData.throttle -= isDownKeyPressed ? 1 : 0;

            moveData.steeringThrow = isRightKeyPressed ? 1 : 0;
            moveData.steeringThrow -= isLeftKeyPressed ? 1 : 0;

        }).Run();
    }
}
