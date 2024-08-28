using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using static UnityEngine.GraphicsBuffer;

public class Beam : MonoBehaviour
{
    private static readonly string ANIMATOR_STATE_PROPERTY = "State";
    private enum State
    {
        Passive = 0,
        Cut = 1,
        Burn = 2,
    };

    [SerializeField] private float _maxDistance = 8f;
    [SerializeField] private LayerMask _environmentLayerMask;
    [SerializeField] private LayerMask _damagableLayerMask;

    private Transform _grub;
    private Transform _bub;
    private Animator _animator;
    private BoxCollider _boxCollider;

    private State _currentState = State.Passive;

    void Start()
    {
        _grub = GameObject.Find(Config.GRUB_GAME_OBJECT_NAME).transform;
        _bub = GameObject.Find(Config.BUB_GAME_OBJECT_NAME).transform;
        _animator = GetComponent<Animator>();
        _boxCollider = GetComponent<BoxCollider>();
    }

    void Update()
    {
        HandleFire();
        CheckForCut();

        UpdateAnimation();
    }

    private void FixedUpdate()
    {
        Move();
        Stretch();
        Rotate();

        Damage();
    }

    private void OnEnable()
    {
        RegisterInputs();
    }

    private void OnDisable()
    {
        UnregisterInputs();
    }

    private void RegisterInputs()
    {
        InputManager.Instance.GameControls.Player.Burn.Enable();
    }

    private void UnregisterInputs()
    {
        InputManager.Instance.GameControls.Player.Burn.Disable();
    }

    private void Move()
    {
        transform.position = _grub.position;
    }

    private void Stretch()
    {
        float distance = Vector3.Distance(_grub.position, _bub.position);
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, distance);
    }

    private void Rotate()
    {
        Vector3 direction = _bub.position - transform.position;
        direction.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0);
    }

    private void HandleFire()
    {
        // TODO rewrite to input events
        bool isFireButtonHeld = InputManager.Instance.GameControls.Player.Burn.ReadValue<float>() > 0.1f;
        if (isFireButtonHeld && _currentState != State.Cut)
        {
            ChangeState(State.Burn);
        }
        else if (_currentState == State.Burn)
        {
            ChangeState(State.Passive);
        }
    }

    private void Damage()
    {
        if (_currentState == State.Burn)
        {
            Vector3 center = transform.TransformPoint(_boxCollider.center);
            Vector3 size = Vector3.Scale(_boxCollider.size, transform.localScale);

            Quaternion rotation = transform.rotation;

            // Perform the overlap check
            Collider[] hitColliders = Physics.OverlapBox(center, size / 2f, rotation, _damagableLayerMask);

            if (hitColliders.Length > 0)
            {
                foreach (Collider collider in hitColliders)
                {
                    Health health = collider.gameObject.GetComponent<Health>();
                    if (health != null)
                    {
                        health.Damage();
                    }
                }
            }
        }
    }

    private void ChangeState(State newState)
    {
        _currentState = newState;
    }

    private void UpdateAnimation()
    {
        _animator.SetInteger(ANIMATOR_STATE_PROPERTY, (int) _currentState);
    }

    private void CheckForCut()
    {
        Vector3 beamYOffset = new Vector3(0, 0.25f, 0);
        Vector3 origin = _grub.position + beamYOffset;
        Vector3 target = _bub.position + beamYOffset;
        float maxDistance = Vector3.Distance(origin, target);

        if(maxDistance > _maxDistance) {
            ChangeState(State.Cut);
            return;
        }

        Vector3 direction = target - origin;
        RaycastHit hit;
        if (Physics.Raycast(origin, direction, out hit, maxDistance, _environmentLayerMask))
        {
            ChangeState(State.Cut);
        }
        else if (_currentState == State.Cut)
        {
            ChangeState(State.Passive);
        }
    }
}
