using System.Collections.Generic;
using UnityEngine;

public class WheelController : MonoBehaviour
{
    public float SpinSpeed = 100f;
    
    public List<WheelNumberData> WheelNumberDataList;
    private void Update()
    {
        transform.Rotate(Vector3.forward, SpinSpeed * Time.deltaTime);
    }
    
    public WheelNumberData GetNumberData(int number)
    {
        return WheelNumberDataList.Find(x => x.Number == number);
    }
}

