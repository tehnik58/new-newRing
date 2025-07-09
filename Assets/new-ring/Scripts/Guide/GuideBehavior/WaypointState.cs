using UnityEngine;

public class WaypointState : GuideState
{
    private float _pauseTimer;
    private bool _isPlayingAudio;
    private Waypoint _currentWaypoint;

    public WaypointState(GuideController controller)
    {
        Controller = controller;
    }

    public override void Enter()
    {
        _currentWaypoint = Controller.Path[Controller.CurrentIndex];
        if (Controller.UseVoice && _currentWaypoint.guideVoice)
        {
            Controller.AudioSource.clip = _currentWaypoint.guideVoice;
            Controller.AudioSource.Play();
            _isPlayingAudio = true;
        }
        else
        {
            _isPlayingAudio = false;
            _pauseTimer = 0f;
        }
    }

    public override void Update()
    {
        if (_isPlayingAudio)
        {
            if (!Controller.AudioSource.isPlaying)
            {
                ProceedToNextWaypoint();
            }
        }
        else
        {
            _pauseTimer += Time.deltaTime;
            if (_pauseTimer >= Controller.PauseDuration)
            {
                ProceedToNextWaypoint();
            }
        }
    }

    public override void Exit()
    {
        if (Controller.AudioSource.isPlaying)
            Controller.AudioSource.Stop();
    }

    private void ProceedToNextWaypoint()
    {
        Controller.CurrentIndex++;
        if (Controller.CurrentIndex < Controller.Path.Count)
            Controller.ChangeState(Controller.MovingState);
        else
            Controller.ChangeState(Controller.IdleState);
    }
}