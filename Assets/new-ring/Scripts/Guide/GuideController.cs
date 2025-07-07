using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GuideController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float pauseDuration = 2f;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private bool useVoice;

    private readonly List<Waypoint> _path = new();
    private Coroutine _moveRoutine;
    private int _currentIndex;
    private Route _route;

    public void Init(Route route)
    {
        _route = route;
    }

    public void StartRoute()
    {
        if(_route == null) return;
        Debug.Log("Starting route");

        foreach (var waypointName in _route.waypointsName)
        {
            if (WaypointsStorage.Waypoints.TryGetValue(waypointName, out var current))
                _path.Add(current);
            else
                Debug.Log("Не найдена точка из маршрута");
        }
        Debug.Log(string.Join(", ", _path));
        
        if (_path.Count > 0 && _moveRoutine == null)
            _moveRoutine = StartCoroutine(FollowPath());
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

            _currentIndex++;
        }

        _moveRoutine = null;
    }
}
