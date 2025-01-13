
public abstract class BehaviorTreeBase
{
    private Node _root = null;

    /// <summary>
    /// Starts the setup the Tree
    /// </summary>
    public void Start()
    {
        _root = SetupTree();
    }

    /// <summary>
    /// Call this in Update Funktion in Unity
    /// Evaluats the Tree
    /// </summary>
    public void Update()
    {
        if (_root != null)
        {
            _root.Evaluate();
        }
    }

    public abstract Node SetupTree();
}
