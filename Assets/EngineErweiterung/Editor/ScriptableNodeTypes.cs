using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Engine/SNodeTypes")]
public class ScriptableNodeTypes : ScriptableObject
{
    [SerializeField]
    private List<string> classNames;

    public List<Type> ClassTypes => classNames.ConvertAll(name => Type.GetType(name));
}
