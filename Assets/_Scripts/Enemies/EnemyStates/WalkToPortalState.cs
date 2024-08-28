using System.Collections.Generic;
using UnityEngine;

public class WalkToPortalState : IEnemyState
{
    private Transform closestPortal;

    public void EnterState(BaseEnemy enemy)
    {
        closestPortal = FindClosestPortal(enemy);
    }

    public void UpdateState(BaseEnemy enemy)
    {
        enemy.GoToPosition(closestPortal.position);

        if (Vector3.Distance(enemy.transform.position, closestPortal.position) < 0.4f)
        {
            enemy.EscapeThroughPortal();
        }
    }

    public void ExitState(BaseEnemy enemy) {}

    public void CheckTransitions(BaseEnemy enemy)
    {
        if (enemy.CheckForDeath())
        {
            return;
        }
    }

    private Transform FindClosestPortal(BaseEnemy enemy)
    {
        Portal[] portals = NavmeshMovement.GetAllPortals();
        List<Transform> portalTransforms = new List<Transform>();
        foreach (Portal portal in portals)
        {
            portalTransforms.Add(portal.transform);
        }
        return NavmeshMovement.FindNearestTarget(enemy.transform.position, portalTransforms, 0.6f);
    }
}