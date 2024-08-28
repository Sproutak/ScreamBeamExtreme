using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow: MonoBehaviour
{
    [SerializeField] private float _followSpeed = 5f;

    private Transform _grub;
    private Transform _bub;

    void Start()
    {
        _grub = GameObject.Find(Config.GRUB_GAME_OBJECT_NAME).transform;
        _bub = GameObject.Find(Config.BUB_GAME_OBJECT_NAME).transform;
    }

    void FixedUpdate()
    {
        Vector3 targetPosition = Vector3.Lerp(_grub.transform.position, _bub.transform.position, 0.5f);

        float distance = Vector3.Distance(transform.position, targetPosition);

        if (distance > 0.01f)
        {
            float step = _followSpeed * distance * Time.fixedDeltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
        }
    }
}
