using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavmeshMovement : MonoBehaviour
{
    private static readonly float DEFAULT_DISTANCE_TO_TARGET = 0.1f;

    [SerializeField] private Vector3 _targetPosition;
    [SerializeField] private LayerMask _unitLayerMask;

    private Movement _movement;
    private Health _health;

    private NavMeshAgent _agent;
    private NavMeshPath _path;
    private float _distanceToTarget = DEFAULT_DISTANCE_TO_TARGET;
    private float _avoidanceRadius = 1f;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _movement = GetComponent<Movement>();
        _health = GetComponent<Health>();
    }

    void Start()
    {
        _path = new NavMeshPath();
    }

    void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (_health.IsAlive() && Vector3.Distance(transform.position, _targetPosition) > _distanceToTarget)
        {
            CalculateDirection();
        }
        else
        {
            StopMovement();
        }
    }

    private void CalculateDirection()
    {
        Vector3 closestTargetPosition = GetClosestPoint(transform.position, _targetPosition, _distanceToTarget);
        bool targetReachable = NavMesh.CalculatePath(transform.position, closestTargetPosition, NavMesh.AllAreas, _path);

        if (targetReachable && _path.corners.Length > 1)
        {
            bool nextTooNear = Vector3.Distance(transform.position, _path.corners[1]) < 0.1f;
            Vector3 cornerToFollow = nextTooNear && _path.corners.Length > 2 ? _path.corners[2] : _path.corners[1];
            cornerToFollow = new Vector3(cornerToFollow.x, 0f, cornerToFollow.z);
            Vector3 direction = (cornerToFollow - transform.position).normalized;

            direction = ApplyAvoidance(direction);

            _movement.SetDirection(direction);
        }
    }

    private Vector3 ApplyAvoidance(Vector3 direction)
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _avoidanceRadius, _unitLayerMask);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject != gameObject)
            {
                Vector3 avoidanceDirection = transform.position - hitCollider.transform.position;
                direction += avoidanceDirection.normalized;
            }
        }

        return direction.normalized;
    }

    private static Vector3 GetClosestPoint(Vector3 position, Vector3 target, float distanceToTarget)
    {
        Vector3 direction = (position - target).normalized;
        return target + direction * distanceToTarget;
    }

    private void StopMovement()
    {
        _movement.SetDirection(Vector3.zero);
    }

    private static float CalculatePathLength(Vector3 start, Vector3 end)
    {
        NavMeshPath path = new NavMeshPath();

        if (NavMesh.CalculatePath(start, end, NavMesh.AllAreas, path))
        {
            float length = 0.0f;
            if (path.status == NavMeshPathStatus.PathComplete)
            {
                for (int i = 1; i < path.corners.Length; i++)
                {
                    length += Vector3.Distance(path.corners[i - 1], path.corners[i]);
                }
            }
            return length;
        }
        return Mathf.Infinity;
    }

    public void SetTargetPosition(Vector3 targetPosition, float distanceToTarget)
    {
        _targetPosition = targetPosition;
        _distanceToTarget = Mathf.Max(distanceToTarget, DEFAULT_DISTANCE_TO_TARGET);
    }

    public static Transform FindNearestTarget(Vector3 position, List<Transform> targets, float distanceToTarget)
    {
        Transform nearestTarget = null;
        float shortestDistance = Mathf.Infinity;

        foreach (Transform target in targets)
        {
            Vector3 closestTarget = GetClosestPoint(position, target.position, distanceToTarget);

            float distance = CalculatePathLength(position, closestTarget);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestTarget = target;
            }
        }

        return nearestTarget;
    }

    public static Chest[] GetAllChests()
    {
        return FindObjectsOfType<Chest>();
    }

    public static Portal[] GetAllPortals()
    {
        return FindObjectsOfType<Portal>();
    }
}
