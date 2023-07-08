using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField] protected float speed = 5f;
    [SerializeField] protected float friction = 0.1f;
    [SerializeField] protected float MAX_SPEED = 5f;
    [SerializeField] protected float radius = 1f;
    [SerializeField] protected Rigidbody2D _rigidbody2D;
    [SerializeField] protected EntityManager entityManager;
    [SerializeField] protected LayerMask obstacleLayerMask;
    protected Vector2 velocity;
    
    protected virtual void Update()
    {
        Move();
    }

    protected virtual void Move()
    {
        Vector2 curr = SeekTargets() * entityManager.weight.x + SeekNonTargets() * entityManager.weight.y + SeekObstacles() * entityManager.weight.z;
        velocity += curr;
        velocity *= (1f - friction);
        velocity = Vector2.ClampMagnitude(velocity, MAX_SPEED);
        _rigidbody2D.MovePosition(_rigidbody2D.position + velocity);
    }

    protected virtual Vector2 SeekTargets()
    {
        Vector2 result = Vector2.zero;
        return result;
    }

    protected virtual Vector2 SeekNonTargets()
    {
        Vector2 result = Vector2.zero;
        return result;
    }

    protected virtual Vector2 SeekRockMinTarget()
    {
        if (entityManager.rockEntities.Count == 0)
            return Vector2.zero;

        Vector2 minDistance = (Vector2)(entityManager.rockEntities[0].transform.position - transform.position);
        if (minDistance.magnitude < 0.5f) 
        {
            entityManager.rockEntities[0].Die();
            return Vector2.zero;
        }
        for (int i = 1; i < entityManager.rockEntities.Count; i++)
        {
            Vector2 distance = (Vector2)(entityManager.rockEntities[i].transform.position - transform.position);
            if (distance.magnitude < 0.5f)
            {
                entityManager.rockEntities[i].Die();
                return Vector2.zero;
            }
            if (distance.magnitude < minDistance.magnitude)
                minDistance = distance;
        }

        return minDistance;
    }

    protected virtual Vector2 SeekPaperMinTarget()
    {
        if (entityManager.paperEntities.Count == 0)
            return Vector2.zero;

        Vector2 minDistance = (Vector2)(entityManager.paperEntities[0].transform.position - transform.position);
        if (minDistance.magnitude < 0.5f) 
        {
            entityManager.paperEntities[0].Die();
            return Vector2.zero;
        }
        for (int i = 1; i < entityManager.paperEntities.Count; i++)
        {
            Vector2 distance = (Vector2)(entityManager.paperEntities[i].transform.position - transform.position);
            if (distance.magnitude < 0.5f) 
            {
                entityManager.paperEntities[i].Die();
                return Vector2.zero;
            }
            if (distance.magnitude < minDistance.magnitude)
                minDistance = distance;
        }

        return minDistance;
    }

    protected virtual Vector2 SeekScissorsMinTarget()
    {
        if (entityManager.scissorsEntities.Count == 0)
            return Vector2.zero;
            
        Vector2 minDistance = (Vector2)(entityManager.scissorsEntities[0].transform.position - transform.position);
        if (minDistance.magnitude < 0.5f) 
        {
            entityManager.scissorsEntities[0].Die();
            return Vector2.zero;
        }
        for (int i = 1; i < entityManager.scissorsEntities.Count; i++)
        {
            Vector2 distance = (Vector2)(entityManager.scissorsEntities[i].transform.position - transform.position);
            if (distance.magnitude < 0.5f) 
            {
                entityManager.scissorsEntities[i].Die();
                return Vector2.zero;
            }
            if (distance.magnitude < minDistance.magnitude)
                minDistance = distance;
        }

        return minDistance;
    }

    protected virtual void HandlePointsTo(EntityManager.Type to, ref Vector2 minDistance, int i)
    {
        Vector2 distance;
        switch (to)
        {
            case EntityManager.Type.Rock:
                distance = SeekRockMinTarget();
                if (distance.magnitude < minDistance.magnitude || i == 0)
                    minDistance = distance;
                break;
            case EntityManager.Type.Paper:
                distance = SeekPaperMinTarget();
                if (distance.magnitude < minDistance.magnitude || i == 0)
                    minDistance = distance;
                break;
            case EntityManager.Type.Scissors:
                distance = SeekScissorsMinTarget();
                if (distance.magnitude < minDistance.magnitude || i == 0)
                    minDistance = distance;
                break;
        }
    }

    protected virtual Vector2 SeekRockNonTarget()
    {
        if (entityManager.rockEntities.Count == 0)
            return Vector2.zero;

        Vector2 result = Vector2.zero;
        float force = 0f;
        for (int i = 0; i < entityManager.rockEntities.Count; i++)
        {
            Vector2 distance = (Vector2)(transform.position - entityManager.rockEntities[i].transform.position);
            if (distance.magnitude < radius)
            {
                force = 1 / Mathf.Pow(distance.magnitude, 2);
                result += distance.normalized * force;
            }
        }

        return result;
    }

    protected virtual Vector2 SeekPaperNonTarget()
    {
        if (entityManager.paperEntities.Count == 0)
            return Vector2.zero;

        Vector2 result = Vector2.zero;
        float force = 0f;
        for (int i = 0; i < entityManager.paperEntities.Count; i++)
        {
            Vector2 distance = (Vector2)(transform.position - entityManager.paperEntities[i].transform.position);
            if (distance.magnitude < radius)
            {
                force = 1 / Mathf.Pow(distance.magnitude, 2);
                result += distance.normalized * force;
            }
        }

        return result;
    }

    protected virtual Vector2 SeekScissorsNonTarget()
    {
        if (entityManager.scissorsEntities.Count == 0)
            return Vector2.zero;

        Vector2 result = Vector2.zero;
        float force = 0f;
        for (int i = 0; i < entityManager.scissorsEntities.Count; i++)
        {
            Vector2 distance = (Vector2)(transform.position - entityManager.scissorsEntities[i].transform.position);
            if (distance.magnitude < radius)
            {
                force = 1 / Mathf.Pow(distance.magnitude, 2);
                result += distance.normalized * force;
            }
        }

        return result;
    }

    protected virtual void HandlePointsFrom(EntityManager.Type from, ref Vector2 result)
    {
        switch (from)
        {
            case EntityManager.Type.Rock:
                result += SeekRockNonTarget();
                break;
            case EntityManager.Type.Paper:
                result += SeekPaperNonTarget();
                break;
            case EntityManager.Type.Scissors:
                result += SeekScissorsNonTarget();
                break;
        }
    }

    protected virtual Vector2 SeekObstacles()
    {
        // 4 raycast to check for obstacles in 4 directions
        Vector2 result = Vector2.zero;
        float force = 0f;
        RaycastHit2D hit;
        Vector2[] directions = new Vector2[2];
        bool vertical = false;
        bool horizontal = false;

        hit = Physics2D.Raycast(transform.position, Vector2.up, 1f, obstacleLayerMask);
        if (hit.collider != null)
        {
            force = 1 / Mathf.Pow(hit.distance, 2);
            result += Vector2.up * force;
            vertical = true;
        }

        hit = Physics2D.Raycast(transform.position, Vector2.down, 1f, obstacleLayerMask);
        if (hit.collider != null)
        {
            force = 1 / Mathf.Pow(hit.distance, 2);
            result += Vector2.down * force;
            vertical = true;
        }

        if (vertical)
        {
            directions[0] = Vector2.left;
            directions[1] = Vector2.right;

            for (int i = 0; i < directions.Length; i++)
            {
                hit = Physics2D.Raycast(transform.position, directions[i], 3f, obstacleLayerMask);
                Debug.DrawRay(transform.position, directions[i] * 3f, Color.red);
                if (hit.collider == null)
                {
                    result -= directions[i] * force;
                    break;
                }
            }
        }
        
        hit = Physics2D.Raycast(transform.position, Vector2.left, 1f, obstacleLayerMask);
        if (hit.collider != null)
        {
            force = 1 / Mathf.Pow(hit.distance, 2);
            result += Vector2.left * force;
            horizontal = true;
        }

        hit = Physics2D.Raycast(transform.position, Vector2.right, 1f, obstacleLayerMask);
        if (hit.collider != null)
        {
            force = 1 / Mathf.Pow(hit.distance, 2);
            result += Vector2.right * force;
            horizontal = true;
        }

        if (horizontal)
        {
            directions[0] = Vector2.up;
            directions[1] = Vector2.down;

            for (int i = 0; i < directions.Length; i++)
            {
                hit = Physics2D.Raycast(transform.position, directions[i], 3f, obstacleLayerMask);
                Debug.DrawRay(transform.position, directions[i] * 3f, Color.red);
                if (hit.collider == null)
                {
                    result -= directions[i] * force;
                    break;
                }
            }
        }

        return -result.normalized * speed * Time.deltaTime;
    }

    public void Die()
    {
        velocity = Vector2.zero;
        gameObject.SetActive(false);
    }
}