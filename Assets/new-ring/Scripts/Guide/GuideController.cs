using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GuideController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float pauseDuration = 2f;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private bool useVoice;

    private List<Waypoint> _path = new();
    private Coroutine _moveRoutine;
    private int _currentIndex;
    private PathfindingManager _pathManager;

    public bool isPaused { get; private set; }
    public Route route;

    void Start()
    {
        if(route == null) return;
        _pathManager = FindObjectOfType<PathfindingManager>();
        
        foreach (var waypointName in route.waypointsName)
            if (_pathManager.WaypointDictionary.TryGetValue(waypointName, out Waypoint current))
                _path.Add(current);
    }

    void Update()
    {
        if (_path.Count > 0 && _moveRoutine == null)
        {
            _moveRoutine = StartCoroutine(FollowPath());
        }
    }

    private IEnumerator FollowPath()
    {
        while (_currentIndex < _path.Count)
        {
            var target = _path[_currentIndex].transform.position;

            while (Vector3.Distance(transform.position, target) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
                yield return null;
            }

            transform.position = target;
            isPaused = true;

            if (useVoice && _path[_currentIndex].guideVoice && audioSource)
            {
                audioSource.clip = _path[_currentIndex].guideVoice;
                audioSource.Play();
                yield return new WaitForSeconds(audioSource.clip.length);
            }
            else
            {
                yield return new WaitForSeconds(pauseDuration);
            }

            isPaused = false;
            _currentIndex++;
        }

        _moveRoutine = null;
    }
}
