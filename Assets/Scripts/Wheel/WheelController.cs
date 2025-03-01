using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WheelController : MonoBehaviour
{
    [SerializeField] private float _spinSpeed = 100f;
    public List<WheelNumberData> WheelNumberDataList;

    private void Update()
    {
        transform.Rotate(Vector3.forward, _spinSpeed * Time.deltaTime);
    }

    public WheelNumberData GetNumberData(int number)
    {
        return WheelNumberDataList.Find(x => x.Number == number);
    }

    public void SetWheelNumberData()
    {
        WheelNumberDataList.Clear();
        TextMeshPro[] textMeshPros = GetComponentsInChildren<TextMeshPro>();

        int[] redNumbers = { 1, 3, 5, 7, 9, 12, 14, 16, 18, 19, 21, 23, 25, 27, 30, 32, 34, 36 };
        int[] blackNumbers = { 2, 4, 6, 8, 10, 11, 13, 15, 17, 20, 22, 24, 26, 28, 29, 31, 33, 35 };

        foreach (var textMeshPro in textMeshPros)
        {
            WheelNumberData wheelNumberData = new WheelNumberData
            {
                NumberTransform = textMeshPro.transform
            };

            if (textMeshPro.text == "00")
            {
                wheelNumberData.Number = -1;
                wheelNumberData.NumberType = ENumberType.Green;
            }
            else if (int.TryParse(textMeshPro.text, out int number))
            {
                wheelNumberData.Number = number;
                if (number == 0)
                {
                    wheelNumberData.NumberType = ENumberType.Green;
                }
                else if (Array.Exists(redNumbers, n => n == number))
                {
                    wheelNumberData.NumberType = ENumberType.Red;
                }
                else if (Array.Exists(blackNumbers, n => n == number))
                {
                    wheelNumberData.NumberType = ENumberType.Black;
                }
            }

            WheelNumberDataList.Add(wheelNumberData);
        }
    }
}