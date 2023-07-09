using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : Entity
{
    private void OnEnable()
    {
        entityManager.rockEntities.Add(this);
        type = EntityManager.Type.Rock;
    }

    private void OnDisable()
    {
        entityManager.rockEntities.Remove(this);
    }

    protected override Vector2 SeekTargets()
    {
        if (entityManager.rockPointsTo.Count == 0)
            return Vector2.zero;
            
        Vector2 minDistance = Vector2.zero;
        for (int i = 0; i < entityManager.rockPointsTo.Count; i++)
            if (entityManager.rockPointsTo[i] != EntityManager.Type.Rock)
                HandlePointsTo(entityManager.rockPointsTo[i], ref minDistance, i);

        return minDistance.normalized;
    }

    protected override Vector2 SeekNonTargets()
    {
        if (entityManager.rockPointsFrom.Count == 0)
            return Vector2.zero;
        
        Vector2 result = Vector2.zero;
        for (int i = 0; i < entityManager.rockPointsFrom.Count; i++)
            HandlePointsFrom(entityManager.rockPointsFrom[i], ref result);

        return result.normalized;
    }
}
