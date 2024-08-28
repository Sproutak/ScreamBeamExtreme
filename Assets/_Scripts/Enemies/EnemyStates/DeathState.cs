using UnityEngine;

public class DeathState : IEnemyState
{
    public void EnterState(BaseEnemy enemy)
    {
        enemy.Die();
    }

    public void UpdateState(BaseEnemy enemy) {}

    public void ExitState(BaseEnemy enemy) {}

    public void CheckTransitions(BaseEnemy enemy) {}
}