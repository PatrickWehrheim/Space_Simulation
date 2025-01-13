using UnityEngine;

public class CheckIsSquadFlying : Node
{
    public override NodeState Evaluate()
    {
        FighterController fighterController = (FighterController)GetData(nameof(FighterController));
        FighterData fighterData = (FighterData)GetData(nameof(SquadData));

        if (fighterController.SquadController.Data.PatrolPositions.Count > 0)
        {
            _state = NodeState.Success;
            return _state;
        }

        return base.Evaluate();
    }
}
