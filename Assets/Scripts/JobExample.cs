using UnityEngine;
using Unity.Jobs;
using Unity.Collections;

public class JobExample : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DoExample();
    }

    private void DoExample()
    {
        NativeArray<float> resultArray = new NativeArray<float>(1, Allocator.TempJob);

        // 1. Instantiate and Initialize Job
        SimpleJob myJob = new SimpleJob
        {
            a = 5f,
            result = resultArray
        };

        // 1-1. Instantiate and Initialize Job2
        SimpleJob2 myJob2 = new SimpleJob2 
        { 
            result = resultArray 
        };

        // 3. Schedule Job1 and Job2
        JobHandle handle = myJob.Schedule();
        JobHandle handle2 = myJob2.Schedule(handle);

        // 4. Complete JobHandle - Other tasks to run in Main Thread in parellel
        // handle.Complete();   // no need to complete first handle since the second handle will do that
        handle2.Complete();
        float resultingValue = resultArray[0];
        Debug.Log($"result: {resultingValue}");

        resultArray.Dispose();
    }

    private struct SimpleJob : IJob
    {
        public float a;
        public NativeArray<float> result;

        public void Execute()
        {
            result[0] = a;
        }
    }

    private struct SimpleJob2 : IJob
    {
        public NativeArray<float> result;

        public void Execute()
        {
            result[0] += 1;
        }
    }
}
