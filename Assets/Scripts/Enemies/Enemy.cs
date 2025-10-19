using UnityEngine;
using System;

public class Enemy : MonoBehaviour
{

    [Header("Enemy Settings")]
    [SerializeField] private EnemyData data;
    public static event Action<EnemyData> OnEnemyReachedEnd;
    public static event Action<Enemy> OnEnemyDestroyed;
    
    [Header("Path Settings")]
    private Path _currentPath;
    private Vector3 _targetPosition;
    private int _currentWaypoint;
    private float _lives;

    private void Awake()
    {
        _currentPath = GameObject.Find("Path1").GetComponent<Path>();
    }

    private void OnEnable()
    {
        _currentWaypoint = 0;
        _targetPosition = _currentPath.GetPosition(_currentWaypoint);
        _lives = data.lives;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _targetPosition, data.speed * Time.deltaTime);
        float relativeDistance = (transform.position - _targetPosition).magnitude;

        if (relativeDistance < 0.1f)
        {
            if (_currentWaypoint < _currentPath.Waypoints.Length - 1)
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
    
    public void TakeDamage(float damage)
    {
        _lives -= damage;
        _lives = Math.Max(_lives, 0);

        if (_lives <= 0)
        {
            OnEnemyDestroyed?.Invoke(this);
            gameObject.SetActive(false);
        }
    }
}
