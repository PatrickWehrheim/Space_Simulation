using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlanetGen))]
public class PlanetGenEditor : Editor
{
    private PlanetGen _planetGen;

    private Editor _shapeEditor;
    private Editor _colorEditor;

    public override void OnInspectorGUI()
    {
        using(var check = new EditorGUI.ChangeCheckScope())
        {
            base.OnInspectorGUI();

            if (check.changed)
                _planetGen.OnBaseInfoUpdate();
        }

        if (GUILayout.Button("Generate Planet"))
        {
            _planetGen.GeneratePlanet();
        }

        DrawSettingsEditor(_planetGen.ShapeSettings, _planetGen.OnSettingsUpdate, ref _planetGen.ShapeSettingsFoldout, ref _shapeEditor);
        DrawSettingsEditor(_planetGen.ColorSettings, _planetGen.OnSettingsUpdate, ref _planetGen.ColorSettingsFoldout, ref _colorEditor);
    }

    private void DrawSettingsEditor(Object settings, System.Action onSettingsUpdate, ref bool foldout, ref Editor editor)
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

                    if (check.changed)
                        onSettingsUpdate?.Invoke();
                }
            }
        }
    }

    private void OnEnable()
    {
        _planetGen = (PlanetGen)target;
    }

}
