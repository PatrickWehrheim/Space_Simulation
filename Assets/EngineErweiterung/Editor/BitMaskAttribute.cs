
using System;
using UnityEngine;

public class BitMaskAttribute : PropertyAttribute
{
    private Type propType;

    public Type PropType => propType;

    public BitMaskAttribute(Type type)
    {
        propType = type;
    }
}
