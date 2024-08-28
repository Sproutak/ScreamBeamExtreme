using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int _maxHealth = 1;
    [SerializeField] private float _minDamageInterval = 0.2f;

    private Animator _animator;

    private int _currentHealth;
    private float _lastDamageTime;

    private void Start()
    {
        _currentHealth = _maxHealth;
        Transform character = transform.Find(Config.CHARACTER_MODEL_PATH);
        if (character != null)
        {
            _animator = character.GetComponent<Animator>();
        }
    }

    public void Damage()
    {
        if (!IsAlive()) return;
        if (Time.time < _lastDamageTime + _minDamageInterval) return;

        _currentHealth--;

        if (IsAlive())
        {
            if (_animator)
            {
                _animator.SetTrigger("Hit");
            }
        }

        _lastDamageTime = Time.time;
    }

    public int GetHealth()
    {
        return _currentHealth;
    }

    public bool IsAlive()
    {
        return _currentHealth > 0;
    }
}
