using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class BaseEnemy : MonoBehaviour
{
    protected IEnemyState currentState;
    protected Dictionary<EnemyStateType, IEnemyState> states;

    private NavmeshMovement _navmeshMovement;
    private Health _health;
    private Animator _animator;
    private Gold _gold;
    private Chest _chestToLoot;

    protected void Awake()
    {
        _navmeshMovement = GetComponent<NavmeshMovement>();
        _health = GetComponent<Health>();
        _animator = transform.Find(Config.CHARACTER_MODEL_PATH).GetComponent<Animator>();
    }

    void Update()
    {
        currentState?.UpdateState(this);
        currentState?.CheckTransitions(this);

        CarryGold();
    }

    protected void CarryGold()
    {
        if (_gold)
        {
            if (_health.IsAlive())
            {
                _gold.transform.position = transform.position;
            }
            else
            {
                _gold.SetGrounded(true);
                _gold = null;
            }
        }
    }

    public void ChangeState(EnemyStateType newStateType)
    {
        if (states.ContainsKey(newStateType))
        {
            currentState?.ExitState(this);
            currentState = states[newStateType];
            currentState.EnterState(this);
        }
    }

    public bool CanTransitionTo(EnemyStateType newStateType)
    {
        return states.ContainsKey(newStateType);
    }

    public void GoToPosition(Vector3 position, float distanceToTarget = 0f)
    {
        _navmeshMovement.SetTargetPosition(position, distanceToTarget); // TODO Add stop distanceToTarget parameter
    }

    public bool CheckForDeath()
    {
        if (!_health.IsAlive()) {
            if (states[EnemyStateType.Dead] != currentState)
            {
                ChangeState(EnemyStateType.Dead);
            }

            return true;
        }
        return false;
    }

    public void Die()
    {
        _animator.SetTrigger("Die");

        GetComponent<Movement>().SetMovementSpeed(0f);
        GetComponent<Collider>().enabled = false;
        Destroy(gameObject, 5f);
    }

    public void EscapeThroughPortal()
    {
        if (_gold)
        {
            // TODO Count stolen gold
            Destroy(_gold.gameObject);
        }
        Destroy(gameObject);
    }

    public void SetGold(Gold gold)
    {
        _gold = gold;
    }

    public Gold GetGold()
    {
        return _gold;
    }

    public void SetChestToLoot(Chest chest)
    {
        _chestToLoot = chest;
    }

    public Chest GetChestToLoot()
    {
        return _chestToLoot;
    }

    public Animator GetAnimator()
    {
        return _animator;
    }
}