using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeWindow
{
    private Rect windowRect;
    private Node windowNode;
    private bool hasParent;

    public Rect WindowRect { get { return windowRect; } set { windowRect = value; } }
    public Node WindowNode { get { return windowNode; } set { windowNode = value; } }
    public bool HasParent { get { return hasParent; } set { hasParent = value; } }

    public NodeWindow()
    {
        windowRect = new Rect();
        windowNode = null;
        hasParent = false;
    }

    public NodeWindow(Rect windowRect, Node windowNode)
    {
        this.windowRect = windowRect;
        this.windowNode = windowNode;
        hasParent = false;
    }
}
