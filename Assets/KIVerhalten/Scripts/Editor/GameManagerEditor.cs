using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : Editor
{
    private GameManager _target;
    private Editor _selectedUnitsEditor;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (_target.SelectedUnitsSettings != null)
            DrawSettingsEditor(_target.SelectedUnitsSettings, ref _target.SelectedUnitsSettingsFoldout, ref _selectedUnitsEditor);
    }

    private void DrawSettingsEditor(Object settings, ref bool foldout, ref Editor editor)
    {
        if (settings != null)
        {
            foldout = EditorGUILayout.InspectorTitlebar(foldout, settings);

            using (var check = new EditorGUI.ChangeCheckScope())
            {
                if (foldout)
                {
                    CreateCachedEditor(settings, null, ref editor);
                    editor.OnInspectorGUI();
                }
            }
        }
    }

    private void OnEnable()
    {
        _target = (GameManager)target;
    }
}
