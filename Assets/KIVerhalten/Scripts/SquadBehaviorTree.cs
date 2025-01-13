using System.Collections.Generic;

public class SquadBehaviorTree : BehaviorTreeBase
{
    private IController _controller;

    public SquadBehaviorTree(IController controller) : base()
    {
        _controller = controller;
    }

    /// <summary>
    /// Setup the behaviour tree
    /// </summary>
    /// <returns>The root node of the tree</returns>
    public override Node SetupTree()
    {
        Node root = new Selector(new List<Node>()
        {
            new TaskDataInitialise(_controller),
            new Sequenzer(new List<Node>
            {
                new CheckEnemyInFOVRange(),
                new TaskChase(),
                new Sequenzer(new List<Node>
                {
                    new CheckEnemyInAttackRange(),
                    new TaskSquadAttack()
                })
            }),
            new Sequenzer(new List<Node>
            {
                new CheckDoPatrol(),
                new TaskShipPatrol()
            }),
            new TaskShipIdle()
        });

        return root;
    }
}
