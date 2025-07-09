using UnityEngine;

public class WaitingForPlayerState : GuideState
{
    private float _waitTimer;

    public WaitingForPlayerState(GuideController controller)
    {
        Controller = controller;
    }

    public override void Enter()
    {
        _waitTimer = 0f;
    }

    public override void Update()
    {
        _waitTimer += Time.deltaTime;
        if (Vector3.Distance(Controller.transform.position, Controller.PlayerTarget.position) <= 
            Controller.MaxDistanceFromPlayer)
        {
            Controller.ChangeState(Controller.MovingState);
        }
    }

    public override void Exit()
    {
    }
}