using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : MonoBehaviour
{
    private static readonly string ANIMATOR_PARAMETER_GROUNDED = "Grounded";

    private Animator _animator;
    private CapsuleCollider _collider;
    private Chest _ownerChest;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _collider = GetComponent<CapsuleCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.gameObject.GetComponent<Player>();
        if (player)
        {
            ReturnToChest();
        }
    }

    private void ReturnToChest()
    {
        _ownerChest.ReturnGold();
        Destroy(gameObject);
    }

    public void SetGrounded(bool grounded)
    {
        _animator.SetBool(ANIMATOR_PARAMETER_GROUNDED, grounded);
        _collider.enabled = grounded;
    }

    public void SetOwnerChest(Chest ownerChest)
    {
        _ownerChest = ownerChest;
    }
}
