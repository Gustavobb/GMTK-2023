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
    [SerializeField] protected GameObject velocityArrowSprite;
    [SerializeField] protected SoundManager soundManager;
    public EntityManager.Type type;
    protected Vector2 velocity;

    protected virtual void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        entityManager = FindObjectOfType<EntityManager>();
        soundManager = FindObjectOfType<SoundManager>();
    }

    protected virtual void FixedUpdate()
    {
        if (!RulesManager.onGame) return;
        Move();
    }

    protected virtual void Move()
    {
        Vector2 curr = SeekTargets() * entityManager.weight.x + SeekNonTargets() * entityManager.weight.y + RandomDirection();
        SeekObstacles(ref curr);
        velocity += curr * speed * Time.deltaTime;
        Debug.DrawRay(transform.position, velocity.normalized, Color.red);
        
        if (velocity != Vector2.zero)
            velocityArrowSprite.transform.RotateAround(transform.position, Vector3.forward, Vector3.SignedAngle(velocityArrowSprite.transform.up, velocity, Vector3.forward) + 90f);

        velocity *= (1f - friction);
        velocity = Vector2.ClampMagnitude(velocity, MAX_SPEED);
        _rigidbody2D.MovePosition(_rigidbody2D.position + velocity);
    }

    protected virtual Vector2 RandomDirection()
    {
        if (velocity.magnitude > 0.1f) return Vector2.zero;

        Random.InitState((int) (Time.time % 30f) + (int)type + (int)transform.position.x + (int)transform.position.y);
        Vector2 result = Random.insideUnitCircle;
        return result;
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
        if (minDistance.magnitude < entityManager.distanceToKill) 
        {
            entityManager.rockEntities[0].Die();
            return Vector2.zero;
        }
        for (int i = 1; i < entityManager.rockEntities.Count; i++)
        {
            Vector2 distance = (Vector2)(entityManager.rockEntities[i].transform.position - transform.position);
            if (distance.magnitude < entityManager.distanceToKill)
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
        if (minDistance.magnitude < entityManager.distanceToKill) 
        {
            entityManager.paperEntities[0].Die();
            return Vector2.zero;
        }
        for (int i = 1; i < entityManager.paperEntities.Count; i++)
        {
            Vector2 distance = (Vector2)(entityManager.paperEntities[i].transform.position - transform.position);
            if (distance.magnitude < entityManager.distanceToKill) 
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
        if (minDistance.magnitude < entityManager.distanceToKill) 
        {
            entityManager.scissorsEntities[0].Die();
            return Vector2.zero;
        }
        for (int i = 1; i < entityManager.scissorsEntities.Count; i++)
        {
            Vector2 distance = (Vector2)(entityManager.scissorsEntities[i].transform.position - transform.position);
            if (distance.magnitude < entityManager.distanceToKill) 
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

    protected virtual void SeekObstacles(ref Vector2 curr)
    {
        RaycastHit2D hit;
        Vector2[] directions = new Vector2[4] { Vector2.up, Vector2.right, Vector2.down, Vector2.left };
        float rotAngle = 0f;
        bool clockwise = Vector2.SignedAngle(Vector2.left, curr) > 180f;

        hit = Physics2D.Raycast(transform.position, curr.normalized, 1f, obstacleLayerMask);
        if (!hit.collider)
            return;

        for (int i = 0; i < 4; i++)
        {
            hit = Physics2D.Raycast(transform.position, directions[i], 1f, obstacleLayerMask);
            if (hit.collider != null)
                rotAngle = clockwise ? -90f : 90f;
        }

        curr = Quaternion.Euler(0, 0, rotAngle) * curr;
    }

    public virtual void Die()
    {
        velocity = Vector2.zero;
        entityManager.Kill(transform);
        SoundManager.instance.Play("Enemy_death");
        gameObject.SetActive(false);
        entityManager.checkWin();
    }
}
