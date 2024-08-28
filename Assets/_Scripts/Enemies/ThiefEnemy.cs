using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThiefEnemy : BaseEnemy
{
    void Start()
    {
        states = new Dictionary<EnemyStateType, IEnemyState>
        {
            { EnemyStateType.Dead, new DeathState() },
            { EnemyStateType.WalkToChest, new WalkToChestState() },
            { EnemyStateType.LootChest, new LootChestState() },
            { EnemyStateType.WalkToPortal, new WalkToPortalState() },
        };

        ChangeState(EnemyStateType.WalkToChest);
    }
}
