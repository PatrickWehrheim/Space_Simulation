
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlaneGen))]
public class PlaneGenEditor : Editor
{
    private PlaneGen _plane;

    public override void OnInspectorGUI()
    {
        using (var check = new EditorGUI.ChangeCheckScope())
        {
            base.OnInspectorGUI();
        }

        if (GUILayout.Button("Create Plane"))
        {
            _plane.CreatePlane();
        }
    }

    private void OnEnable()
    {
        _plane = (PlaneGen)target;

        _plane.MeshFilter = _plane.gameObject.GetComponent<MeshFilter>();
        _plane.Renderer = _plane.gameObject.GetComponent<Renderer>();
    }
}
