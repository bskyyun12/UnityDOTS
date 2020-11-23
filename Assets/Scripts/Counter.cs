using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Counter : MonoBehaviour
{
    [SerializeField] Text text = default;
    float count;

    void FixedUpdate()
    {
        count += Time.fixedDeltaTime;
        text.text = count.ToString("F2");
    }
}
