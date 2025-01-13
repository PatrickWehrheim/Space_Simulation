using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class NodeCreationDrawer
{
    private List<Type> compositNodeTypes, leafNodeTypes;
	public List<Type> CompositNodeTypes => compositNodeTypes;


    public NodeCreationDrawer()
	{
		compositNodeTypes = new List<Type>();
		leafNodeTypes = new List<Type>();

		FindScripts();
	}


	private void FindScripts()
	{
#if UNITY_EDITOR
		//WARNING: Nur im Editor verfügbar!
		string[] guids = AssetDatabase.FindAssets("t: ScriptableNodeTypes"); //t: Sucht nach Typ

		List<Type> nodeTypes;
		ScriptableNodeTypes nodeTypesSO;

		foreach (string guid in guids)
		{
			nodeTypesSO = AssetDatabase.LoadAssetAtPath<ScriptableNodeTypes>(AssetDatabase.GUIDToAssetPath(guid));
			nodeTypes = nodeTypesSO.ClassTypes;

			foreach (Type type in nodeTypes)
			{
				if (type == null) continue;

				if (type.BaseType == typeof(Node) && !compositNodeTypes.Contains(type))
				{
					compositNodeTypes.Add(type);
				}
                else if (type.BaseType == typeof(Node) && !leafNodeTypes.Contains(type))
                {
                    leafNodeTypes.Add(type);
                }
            }
		}
#endif
	}

	public void DrawNodeCreationButtons(WindowDrawer windowDrawer)
	{
		for (int i = 0; i < compositNodeTypes.Count; i++)
		{
			if (GUI.Button(new Rect(5, 40 * i + 30, 150, 40), compositNodeTypes[i].FullName))
			{
				windowDrawer.AddWindow(new Vector2(50, 50), (Node)Activator.CreateInstance(compositNodeTypes[i]));
			}
		}
        for (int i = 0; i < leafNodeTypes.Count; i++)
        {
            if (GUI.Button(new Rect(205, 40 * i + 30, 150, 40), leafNodeTypes[i].FullName))
            {
                windowDrawer.AddWindow(new Vector2(50, 50), (Node)Activator.CreateInstance(leafNodeTypes[i]));
            }
        }
    }
}
