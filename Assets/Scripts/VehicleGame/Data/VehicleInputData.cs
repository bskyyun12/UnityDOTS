﻿using Unity.Entities;
using UnityEngine;

[GenerateAuthoringComponent]
public struct VehicleInputData : IComponentData
{
    public KeyCode upKey;
    public KeyCode downKey;
    public KeyCode rightKey;
    public KeyCode leftKey;
}
