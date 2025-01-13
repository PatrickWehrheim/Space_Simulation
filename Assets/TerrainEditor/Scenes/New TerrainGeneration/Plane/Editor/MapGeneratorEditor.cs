using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorEditor : Editor
{
    private MapGenerator _mapGenerator;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (_mapGenerator.AutoUpdate)
        {
            _mapGenerator.DrawMapInEditor();
        }

        if (GUILayout.Button("Generate Plane"))
        {
            _mapGenerator.DrawMapInEditor();
        }
    }

    private void OnEnable()
    {
        _mapGenerator = (MapGenerator)target;
    }
}
