using System;
using UnityEngine;

[Serializable]
public class WheelNumberData
{
    public int Number;

    public ENumberType NumberType;

    public Transform NumberTransform;
}

public enum ENumberType
{
    Red = 0,
    Black = 1,
    Green = 2,
    None = 3,
}