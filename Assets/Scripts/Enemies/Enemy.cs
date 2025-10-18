using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField] private EnemyData data;
    
    [SerializeField] private float moveSpeed = 3f;
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
                gameObject.SetActive(false);
            }
        }
    }
}
