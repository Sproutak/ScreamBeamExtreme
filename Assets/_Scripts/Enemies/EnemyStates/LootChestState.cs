using UnityEngine;

public class LootChestState : IEnemyState
{
    private static readonly float TIME_TO_LOOT = 0.415f;
    private static readonly float TIME_TO_FINISH = 0.65f;
    private static readonly string ANIMATOR_PARAMETER_LOOT = "Loot";

    private float startTime;

    public void EnterState(BaseEnemy enemy)
    {
        startTime = Time.time;

        enemy.GetAnimator().SetTrigger(ANIMATOR_PARAMETER_LOOT);
    }

    public void UpdateState(BaseEnemy enemy)
    {
        if (TimeSpent(TIME_TO_LOOT) && !enemy.GetGold())
        {
            Gold gold = enemy.GetChestToLoot().CreateGold();
            if (gold)
            {
                enemy.SetGold(gold);
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

        if (enemy.GetGold() && TimeSpent(TIME_TO_FINISH) && enemy.CanTransitionTo(EnemyStateType.WalkToPortal))
        {
            enemy.ChangeState(EnemyStateType.WalkToPortal);
            return;
        }

        if (!enemy.GetGold() && TimeSpent(TIME_TO_FINISH) && enemy.CanTransitionTo(EnemyStateType.WalkToChest))
        {
            enemy.ChangeState(EnemyStateType.WalkToChest);
            return;
        }
    }

    private bool TimeSpent(float time)
    {
        return Time.time >= startTime + time;
    }
}