using System.Collections.Generic;

public class Node
{
    protected NodeState _state;

    private Dictionary<string, object> _dictionary = new Dictionary<string, object>();

    public Node Parent { get; set; }

    public List<Node> Children { get; set; } = new List<Node>();

    /// <summary>
    /// Simple Constructor
    /// </summary>
    public Node()
    {
        Parent = null;
    }

    /// <summary>
    /// Constructor where chlidren can be set
    /// </summary>
    /// <param name="children"></param>
    public Node(List<Node> children)
    {
        foreach (Node child in children)
        {
            Attach(child);
        }
    }

    /// <summary>
    /// Attach a childnode to this 
    /// </summary>
    /// <param name="child">Node to attach</param>
    private void Attach(Node child)
    {
        child.Parent = this;
        Children.Add(child);
    }

    /// <summary>
    /// Sets data to save in this node
    /// </summary>
    /// <param name="data">Key of the data</param>
    /// <param name="value">Data to save</param>
    public void SetData(string data, object value)
    {
        if (_dictionary.ContainsKey(data))
        {
            _dictionary[data] = value;
        }
        else
        {
            _dictionary.Add(data, value);
        }
    }

    /// <summary>
    /// Search for the given key and returns the result
    /// </summary>
    /// <param name="key">Key where to find the data</param>
    /// <returns>The value of the given key if found</returns>
    public object GetData(string key)
    {
        if (_dictionary.TryGetValue(key, out var value))
        {
            return value;
        }

        for (Node parent = Parent; parent != null; parent = parent.Parent)
        {
            value = parent.GetData(key);
            if (value != null)
            {
                return value;
            }
        }

        return null;
    }

    /// <summary>
    /// Delets the data for the given key
    /// </summary>
    /// <param name="key">Key where to find the data</param>
    /// <returns>true: If succeded</returns>
    public bool ClearData(string key)
    {
        if (_dictionary.ContainsKey(key))
        {
            _dictionary.Remove(key);
            return true;
        }

        for (Node parent = Parent; parent != null; parent = parent.Parent)
        {
            bool flag = parent.ClearData(key);
            if (flag)
            {
                return flag;
            }
        }

        return false;
    }

    /// <summary>
    /// Gets the root node of the tree
    /// </summary>
    /// <returns>Node: The root node of the tree</returns>
    public Node GetRoot()
    {
        if (Parent == null)
        {
            return this;
        }

        Node root = Parent.GetRoot();
        return root;
    }

    /// <summary>
    /// Evaluates if the funktion of this node is failed, succeded or running
    /// </summary>
    /// <returns>NodeState: The state this node is</returns>
    public virtual NodeState Evaluate()
    {
        _state = NodeState.Failed;
        return _state;
    }
}
