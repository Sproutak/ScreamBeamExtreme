using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    private static readonly string ANIMATOR_PARAMETER_TOTAL = "Total";
    private static readonly string ANIMATOR_PARAMETER_DROP_GOLD = "DropGold";
    private static readonly string ANIMATOR_PARAMETER_RETURN_GOLD = "ReturnGold";

    [SerializeField] private int _goldTotal = 5;
    [SerializeField] private Gold _goldPrefab;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _animator.SetInteger(ANIMATOR_PARAMETER_TOTAL, _goldTotal);
    }

    public Gold CreateGold()
    {
        if (_goldTotal > 0)
        {
            Gold gold = Instantiate(_goldPrefab);
            gold.SetOwnerChest(this);

            _goldTotal--;
            _animator.SetInteger(ANIMATOR_PARAMETER_TOTAL, _goldTotal);
            _animator.SetTrigger(ANIMATOR_PARAMETER_DROP_GOLD);

            return gold;
        }

        return null;
    }

    public void ReturnGold()
    {
        _goldTotal++;
        _animator.SetInteger(ANIMATOR_PARAMETER_TOTAL, _goldTotal);
        _animator.SetTrigger(ANIMATOR_PARAMETER_RETURN_GOLD);
    }

    public int GetTotalGold()
    {
        return _goldTotal;
    }
}
