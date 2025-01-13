
public class TaskFlyInFormation : Node
{
    public override NodeState Evaluate()
    {
        FighterController fighterController = (FighterController)GetData(nameof(FighterController));
        FighterData fighterData = (FighterData)GetData(nameof(FighterData));

        SquadController squadController = fighterController.gameObject.GetComponent<SquadController>();
        if (squadController != null)
            squadController.IsAttacking = false;

        fighterController.SeperationValue = 1.5f;
        fighterController.MoveToPoint(fighterController.SquadController.Position);

        _state = NodeState.Running;
        return _state;
    }
}
