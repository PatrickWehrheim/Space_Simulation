using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FancyScript))]
public class FancyScriptEditor : Editor
{
    private FancyScript _fancyScript;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Space(20f);
        GUILayout.Label("Custom Inspector", EditorStyles.boldLabel);

        if (GUILayout.Button("Do Stuff!"))
        {
            _fancyScript.RecalculateRotation();
            _fancyScript.RecalculatePosition();
        }
    }

    private void OnEnable()
    {
        _fancyScript = (FancyScript)target;
    }
}
