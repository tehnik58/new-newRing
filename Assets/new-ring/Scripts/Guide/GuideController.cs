using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
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

    void OnEnable()
    {
        _pathManager = FindObjectOfType<PathfindingManager>();
        SetPathFromManager();
    }

    void OnValidate()
    {
        SetPathFromManager();
        if (_path.Count > 0)
            transform.position = _path[0].transform.position;
        _currentIndex = 0;
    }

    void Update()
    {
        if (Application.isPlaying && _path.Count > 0 && _moveRoutine == null)
        {
            _moveRoutine = StartCoroutine(FollowPath());
        }
    }

    private void SetPathFromManager()
    {
        _path.Clear();
        if (_pathManager == null) return;
        foreach (var segment in _pathManager.FinalPath)
            _path.AddRange(segment);
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
