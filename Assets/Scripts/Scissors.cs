using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scissors : Entity
{
    private void OnEnable()
    {
        entityManager.scissorsEntities.Add(this);
        type = EntityManager.Type.Scissors;
    }

    private void OnDisable()
    {
        entityManager.scissorsEntities.Remove(this);
    }

    protected override Vector2 SeekTargets()
    {
        if (entityManager.scissorsPointsTo.Count == 0)
            return Vector2.zero;

        Vector2 minDistance = Vector2.zero;
        for (int i = 0; i < entityManager.scissorsPointsTo.Count; i++)
            if (entityManager.scissorsPointsTo[i] != EntityManager.Type.Scissors)
                HandlePointsTo(entityManager.scissorsPointsTo[i], ref minDistance, i);

        return minDistance.normalized;
    }
    
    protected override Vector2 SeekNonTargets()
    {
        if (entityManager.scissorsPointsFrom.Count == 0)
            return Vector2.zero;
        
        Vector2 result = Vector2.zero;
        for (int i = 0; i < entityManager.scissorsPointsFrom.Count; i++)
            HandlePointsFrom(entityManager.scissorsPointsFrom[i], ref result);

        return result.normalized;
    }
}
