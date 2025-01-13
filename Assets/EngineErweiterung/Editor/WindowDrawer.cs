using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WindowDrawer
{
    private List<NodeWindow> windows;

    private readonly Vector2 nodeWindowSize = new Vector2(250, 80);

    private int selectedRootNode = -1;
    private NodeWindow rootNode;

    private List<Type> compositNodeTypes;

    private ConnectionHandler connectionHandler;

    public WindowDrawer(List<Type> compositNodeTypes)
    {
        windows = new List<NodeWindow>();
        this.compositNodeTypes = compositNodeTypes;

        rootNode = new NodeWindow(new Rect(10, 10, nodeWindowSize.x, nodeWindowSize.y), 
            compositNodeTypes.Count > 0 ? (Node)Activator.CreateInstance(compositNodeTypes[0]) : null);

        connectionHandler = new ConnectionHandler();
    }

    public void AddWindow(Vector2 position, Node node)
    {
        windows.Add(new NodeWindow(new Rect(position.x, position.y, nodeWindowSize.x, nodeWindowSize.y), node));
    }

    public void RedrawWindows(TreeWindow treeWindow)
    {
        treeWindow.BeginWindows();

        rootNode.WindowRect = GUI.Window(-1, rootNode.WindowRect, DrawRootNodeWindow, "RootNode");

        for (int i = 0; i < windows.Count; i++)
        {
            windows[i].WindowRect = GUI.Window(i, windows[i].WindowRect, DrawNodeWindow, windows[i].WindowNode.ToString());
        }

        treeWindow.EndWindows();

        connectionHandler.DrawConnections();
    }

    private void DrawNodeWindow(int windowIndex)
    {
        if (GUI.Button(new Rect(0, 0, 20, 20), "X")) //Close Button
        {
            windows.RemoveAt(windowIndex);

            return;
        }

        DrawAddAndRemoveButton(windows[windowIndex]);

        if (GUI.Button(new Rect(0, 47.5f, 20, 20), "="))
        {
            connectionHandler.UpdateConnection(windows[windowIndex]);
        }

        //Immer ans Ende, da andere Funktion nicht erkannt werden!
        GUI.DragWindow();
    }

    private void DrawRootNodeWindow(int windowIndex)
    {
        if (selectedRootNode != (selectedRootNode = EditorGUILayout.Popup(selectedRootNode, compositNodeTypes.ConvertAll(type => type.ToString()).ToArray())))
        {
            rootNode.WindowNode = (Node)Activator.CreateInstance(compositNodeTypes[selectedRootNode]);
        }

        DrawAddAndRemoveButton(rootNode);

        GUI.DragWindow();
    }

    private void DrawAddAndRemoveButton(NodeWindow window)
    {
        if (GUI.Button(new Rect(nodeWindowSize.x - 20, 35, 20, 20), "+"))
        {
            connectionHandler.SetParentNode(window, true);
        }
        if (GUI.Button(new Rect(nodeWindowSize.x - 20, 60, 20, 20), "-"))
        {
            connectionHandler.SetParentNode(window, false);
        }
    }
}
