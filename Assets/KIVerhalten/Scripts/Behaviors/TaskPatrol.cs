


using System.Collections.Generic;
using UnityEngine;

public class TaskPatrol : Node
{
    public override NodeState Evaluate()
    {
        IController controller = (IController)GetData(nameof(IController));
        IData data = (IData)GetData(nameof(IData));

        //if (controller.Fighters.Count == 0)
        //{
        //    return base.Evaluate();
        //}

        //data.ResultDirection = controller.GetDirectionToMove(data.SteeringBehaviours, ref data);

        //Vector3 nextPatrolPosition = controller.GetNextPatrolPoint();
        //nextPatrolPosition.y = 0;

        //if (Vector3.Distance(controller.transform.position, nextPatrolPosition) <= data.TargetRechedThreshold)
        //{
        //    data.PatrolPositions.RemoveAt(0);
        //}

        ////List<Vector3> fighterLocalPosition = new List<Vector3>();
        ////foreach(FighterController fighterController in squadController.Fighters)
        ////{
        ////    fighterLocalPosition.Add(fighterController.transform.position);
        ////}

        //controller.transform.LookAt(nextPatrolPosition);

        //Vector3 direction = new Vector3(0, 0, data.MoveSpeed) * Time.deltaTime;
        //controller.transform.Translate(direction);

        ////for (int i = 0; i < fighterLocalPosition.Count; i++)
        ////{
        ////    FighterController fighterController = squadController.Fighters[i];
        ////    fighterLocalPosition[i] += squadController.transform.position - fighterController.transform.position;
        ////    fighterController.transform.position = fighterLocalPosition[i];
        ////}

        GetRoot().SetData(nameof(IData), data);

        _state = NodeState.Running;
        return _state;
    }
}