using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FighterSpawner))]
public class FighterSpawnerEditor : Editor
{
    private FighterSpawner _target;
    private Editor _selectedUnitsEditor;
    private Editor _kiBehaviorEditor;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (_target.KIBehaviorSettings != null)
            DrawSettingsEditor(_target.KIBehaviorSettings, ref _target.KIBehaviorSettingsFoldout, ref _kiBehaviorEditor);
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
        _target = (FighterSpawner)target;
    }
}
