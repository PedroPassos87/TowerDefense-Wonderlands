using UnityEngine;
using System;

public class Enemy : MonoBehaviour
{

    [Header("Enemy Settings")]
    [SerializeField] private EnemyData data;
    public static event Action<EnemyData> OnEnemyReachedEnd;
    
    [Header("Path Settings")]
    private Path _currentPath;
    private Vector3 _targetPosition;
    private int _currentWaypoint;

    private void Awake()
    {
        _currentPath = GameObject.Find("Path1").GetComponent<Path>();
    }

    private void OnEnable()
    {
        _currentWaypoint = 0;
        _targetPosition = _currentPath.GetPosition(_currentWaypoint);
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _targetPosition, data.speed * Time.deltaTime);    
        float relativeDistance = (transform.position - _targetPosition).magnitude;

        if(relativeDistance < 0.1f)
        {
            if(_currentWaypoint < _currentPath.Waypoints.Length - 1)
            {
                _currentWaypoint++;
                _targetPosition = _currentPath.GetPosition(_currentWaypoint);
            }
            else
            {
                OnEnemyReachedEnd?.Invoke(data);
                gameObject.SetActive(false);
            }
        }
    }
}
