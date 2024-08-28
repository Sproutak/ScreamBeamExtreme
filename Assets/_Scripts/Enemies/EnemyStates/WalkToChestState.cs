using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkToChestState : IEnemyState
{
    private static readonly float CHEST_DISTANCE_INTERACTIBLE = 0.65f;

    public void EnterState(BaseEnemy enemy)
    {
        FindClosestChest(enemy);
    }

    public void UpdateState(BaseEnemy enemy)
    {
        if(enemy.GetChestToLoot())
        {
            if (enemy.GetChestToLoot().GetTotalGold() <= 0)
            {
                FindClosestChest(enemy);
            }
            else
            {
                enemy.GoToPosition(enemy.GetChestToLoot().transform.position, CHEST_DISTANCE_INTERACTIBLE);
            }
        }
    }

    public void ExitState(BaseEnemy enemy) {}

    public void CheckTransitions(BaseEnemy enemy)
    {
        if (enemy.CheckForDeath())
        {
            return;
        }

        // TODO Change to wander or something
        if (enemy.CanTransitionTo(EnemyStateType.WalkToPortal) && !enemy.GetChestToLoot())
        {
            enemy.ChangeState(EnemyStateType.WalkToPortal);
            return;
        }

        if (enemy.CanTransitionTo(EnemyStateType.LootChest) && Vector3.Distance(enemy.transform.position, enemy.GetChestToLoot().transform.position) < CHEST_DISTANCE_INTERACTIBLE)
        {
            enemy.ChangeState(EnemyStateType.LootChest);
            return;
        }
    }

    private void FindClosestChest(BaseEnemy enemy)
    {
        Chest[] chests = NavmeshMovement.GetAllChests();
        List<Transform> fullChestsTransforms = new List<Transform>();
        foreach (Chest chest in chests)
        {
            if (chest.GetTotalGold() > 0)
            {
                fullChestsTransforms.Add(chest.transform);
            }
        }

        Transform closestChest = NavmeshMovement.FindNearestTarget(enemy.transform.position, fullChestsTransforms, 0.6f);
        enemy.SetChestToLoot(closestChest ? closestChest.GetComponent<Chest>() : null);
    }
}