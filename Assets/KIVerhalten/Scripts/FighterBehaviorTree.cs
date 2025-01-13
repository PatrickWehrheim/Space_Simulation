using System.Collections.Generic;

public class FighterBehaviorTree : BehaviorTreeBase
{
    private FighterController _controller;

    public FighterBehaviorTree(FighterController controller) : base()
    {
        _controller = controller;
    }

    /// <summary>
    /// Setup the behaviour tree
    /// </summary>
    /// <returns>The root node of the tree</returns>
    public override Node SetupTree()
    {
        Node root = new Selector(new List<Node>
        {
            new TaskFighterInitialise(_controller),
            new Selector(new List<Node>
            {
                new TaskFighterAttack(),
                new TaskFlyInFormation(),
            }),
            //new TaskIdle()
        });

        return root;
    }
}
