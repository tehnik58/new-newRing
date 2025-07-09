using UnityEngine;
using System.Collections.Generic;

public class GuideController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float pauseDuration = 2f;
    [SerializeField] private float maxDistanceFromPlayer = 5f;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private bool useVoice;
    [SerializeField] private Animator animator;
    
    private readonly List<Waypoint> _path = new();
    private Route _route;
    private int _currentIndex;
    private Transform _playerTarget;
    private GuideState _currentState;
    
    // Хранение состояний
    public MovingState MovingState { get; private set; }
    public WaitingForPlayerState WaitingForPlayerState { get; private set; }
    public WaypointState WaypointState { get; private set; }
    public IdleState IdleState { get; private set; }

    // Свойства для доступа из состояний
    public List<Waypoint> Path => _path;
    public int CurrentIndex 
    { 
        get => _currentIndex; 
        set 
        { 
            if (value >= 0 && value <= _path.Count)
                _currentIndex = value;
            else
                Debug.LogError($"Invalid CurrentIndex: {value}. Path count: {_path.Count}");
        } 
    }
    public Transform PlayerTarget => _playerTarget;
    public float MoveSpeed => moveSpeed;
    public float MaxDistanceFromPlayer => maxDistanceFromPlayer;
    public float PauseDuration => pauseDuration;
    public AudioSource AudioSource => audioSource;
    public bool UseVoice => useVoice;

    public void Init(Route route, Transform playerTarget)
    {
        if (route == null)
        {
            Debug.LogError("Route is null");
            return;
        }

        if (playerTarget == null)
        {
            Debug.LogError("PlayerTarget is null");
            return;
        }
        
        _route = route;
        _playerTarget = playerTarget;
        
        MovingState = new MovingState(this);
        IdleState = new IdleState(this);
        WaypointState = new WaypointState(this);
        WaitingForPlayerState = new WaitingForPlayerState(this);
    }

    public void StartRoute()
    {
        if (!CreatePath()) return;
        
        _currentState = MovingState;
        _currentState.Enter();
    }

    private bool CreatePath()
    {
        _path.Clear();
        foreach (var waypointName in _route.waypointsName)
        {
            if (WaypointsStorage.Waypoints.TryGetValue(waypointName, out var current))
                _path.Add(current);
            else
                Debug.LogWarning($"Waypoint not found: {waypointName}");
        }
        return _path.Count > 0;
    }

    private void Update()
    {
        _currentState?.Update();
        UpdateAnimator();
    }

    public void ChangeState(GuideState newState)
    {
        _currentState?.Exit();
        _currentState = newState;
        _currentState?.Enter();
    }

    private void UpdateAnimator()
    {
        if (!animator) return;

        animator.SetBool("IsMoving", _currentState is MovingState);
        animator.SetBool("IsWaiting", _currentState is WaitingForPlayerState);
        animator.SetBool("IsPaused", _currentState is WaypointState);
        animator.SetBool("IsIdle", _currentState is IdleState);
    }
}