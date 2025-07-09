using UnityEngine;

public class MovingState : GuideState
{
    private Vector3 _targetPosition;

    public MovingState(GuideController controller)
    {
        Controller = controller;
    }

    public override void Enter()
    {
        if (Controller.CurrentIndex >= Controller.Path.Count)
        {
            Controller.ChangeState(Controller.IdleState);
            return;
        }
        _targetPosition = Controller.Path[Controller.CurrentIndex].transform.position;
    }

    public override void Update()
    {
        if (Controller.CurrentIndex >= Controller.Path.Count)
        {
            Controller.ChangeState(Controller.IdleState);
            return;
        }

        Controller.transform.position = Vector3.MoveTowards(
            Controller.transform.position, _targetPosition, Controller.MoveSpeed * Time.deltaTime);

        if (Vector3.Distance(Controller.transform.position, Controller.PlayerTarget.position) > 
            Controller.MaxDistanceFromPlayer)
        {
            Controller.ChangeState(Controller.WaitingForPlayerState);
            return;
        }

        if (Vector3.Distance(Controller.transform.position, _targetPosition) <= 0.01f)
        {
            Controller.ChangeState(Controller.WaypointState);
        }
    }

    public override void Exit()
    {
    }
}