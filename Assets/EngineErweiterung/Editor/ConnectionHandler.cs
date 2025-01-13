using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ConnectionHandler
{
    private bool addConnection;
    private NodeWindow connectionParent;

    private List<WindowConnections> connectedWindows;

    public ConnectionHandler()
    {
        addConnection = true;
        connectionParent = new NodeWindow();
        connectedWindows = new List<WindowConnections>();
    }

    public void DrawConnections()
    {
        foreach (var connection in connectedWindows)
        {
            DrawNodeCurve(connection.Parent.WindowRect, connection.Child.WindowRect);
        }
    }

    private void DrawNodeCurve(Rect start, Rect end) 
    {
        Vector3 startPos = new Vector3(start.x + start.width, start.y + start.height * 0.5f, 0);
        Vector3 endPos = new Vector3(end.x, end.y + end.height * 0.5f, 0);
        Vector3 startTan = startPos + Vector3.right * 50f;
        Vector3 endTan = endPos + Vector3.left * 50f;

        Color shadowColor = new Color(0, 0, 0, 0.06f);

        for (int i = 0; i < 3; i++)
        {
            Handles.DrawBezier(startPos, endPos, startTan, endTan, shadowColor, null, (i + 1f) * 5);
        }

        Handles.DrawBezier(startPos, endPos, startTan, endTan, Color.black, null, 1f);
    }

    public void SetParentNode(NodeWindow nodeWindow, bool addConnection)
    {
        this.addConnection = addConnection;
        connectionParent = nodeWindow;
    }

    public void ConnectNodes(NodeWindow nodeWindow)
    {
        WindowConnections connection = new WindowConnections(connectionParent, nodeWindow);


        if (nodeWindow.HasParent || connectionParent.WindowNode == null || connectionParent.WindowNode == nodeWindow.WindowNode) return;

        connectedWindows.Add(connection);

        nodeWindow.HasParent = true;
        connectionParent = null;
    }

    public void TryRemoveConnection(NodeWindow parent, NodeWindow child)
    {
        WindowConnections connection;

        if (!child.HasParent || parent.WindowNode == null || parent.WindowNode == child.WindowNode) return;

        for (int i = connectedWindows.Count - 1; i >= 0; i--)
        {
            connection = connectedWindows[i];

            if (connection.Parent == parent && connection.Child == child)
            {
                connectedWindows.RemoveAt(i);
                child.HasParent = false;
                break;
            }
        }
    }

    public void UpdateConnection(NodeWindow nodeWindow)
    {
        if (addConnection)
        {
            ConnectNodes(nodeWindow);
        }
        else
        {
            TryRemoveConnection(connectionParent, nodeWindow);
            connectionParent = null;
        }
    }

    private class WindowConnections
    {
        public NodeWindow Parent;
        public NodeWindow Child;

        public WindowConnections(NodeWindow parent, NodeWindow child)
        {
            Parent = parent;
            Child = child;
        }
    }
}
