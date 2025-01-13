
using System.Collections.Generic;

public class ShipBehaviourTree : BehaviorTreeBase
{
    private IController _controller;

    public ShipBehaviourTree(IController controller)
    {
        _controller = controller;
    }

    public override Node SetupTree()
    {
        Node root = new Selector(new List<Node>
        {
            new TaskDataInitialise(_controller),
            new Sequenzer(new List<Node>
            {
                new Sequenzer(new List<Node> 
                {
                    new TaskDelayNextTasks(0.5f),
                    new CheckEnemyInFOVRange(),
                }),
                new Sequenzer(new List<Node>
                {
                    new CheckEnemyInAttackRange(),
                    new TaskShipAttack()
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
