using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paper : Entity
{
    private void OnEnable()
    {
        entityManager.paperEntities.Add(this);
    }

    private void OnDisable()
    {
        entityManager.paperEntities.Remove(this);
    }

    protected override Vector2 SeekTargets()
    {
        if (entityManager.paperPointsTo.Count == 0)
            return Vector2.zero;

        Vector2 minDistance = Vector2.zero;
        for (int i = 0; i < entityManager.paperPointsTo.Count; i++)
            if (entityManager.paperPointsTo[i] != EntityManager.Type.Paper)
                HandlePointsTo(entityManager.paperPointsTo[i], ref minDistance, i);

        return minDistance.normalized * speed * Time.deltaTime;
    }

    protected override Vector2 SeekNonTargets()
    {
        if (entityManager.paperPointsFrom.Count == 0)
            return Vector2.zero;
        
        Vector2 result = Vector2.zero;
        for (int i = 0; i < entityManager.paperPointsFrom.Count; i++)
            HandlePointsFrom(entityManager.paperPointsFrom[i], ref result);

        return result.normalized * speed * Time.deltaTime;
    }
}
