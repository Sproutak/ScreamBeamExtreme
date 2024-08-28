using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    private static readonly string ANIMATOR_MOVE_PARAMETER = "Move";

    [SerializeField] private float _movementSpeed = 5f;
    private Animator _animator;

    private Rigidbody _rb;
    private float _currentMovementSpeed;
    private Vector3 _direction;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _animator = transform.Find(Config.CHARACTER_MODEL_PATH).GetComponent<Animator>();

        _currentMovementSpeed = _movementSpeed;
    }

    private void FixedUpdate()
    {
        MoveAndRotate();
    }

    private void MoveAndRotate()
    {
        // Move
        _rb.velocity = _direction * _currentMovementSpeed;

        // Rotate
        Vector3 direction = _direction.normalized;
        if (direction.sqrMagnitude > Mathf.Epsilon)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            float rotationSpeed = Quaternion.Angle(transform.rotation, targetRotation) * 10f;

            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }

        // Animation
        _animator.SetBool(ANIMATOR_MOVE_PARAMETER, _rb.velocity.sqrMagnitude > Mathf.Epsilon);
    }

    public void SetDirection(Vector3 direction)
    {
        _direction = direction;
    }

    public void SetMovementSpeed (float speed)
    {
        _currentMovementSpeed = speed;
    }
}