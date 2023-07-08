using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    [SerializeField] private EntityManager.Type type;

    private void OnEnable()
    {
        switch (type)
        {
            case EntityManager.Type.Paper:
                entityManager.paperEntities.Add(this);
                break;
            case EntityManager.Type.Scissors:
                entityManager.scissorsEntities.Add(this);
                break;
            case EntityManager.Type.Rock:
                entityManager.rockEntities.Add(this);
                break;
        }
    }

    private void OnDisable()
    {
        switch (type)
        {
            case EntityManager.Type.Paper:
                entityManager.paperEntities.Remove(this);
                break;
            case EntityManager.Type.Scissors:
                entityManager.scissorsEntities.Remove(this);
                break;
            case EntityManager.Type.Rock:
                entityManager.rockEntities.Remove(this);
                break;
        }
    }

    protected override void Move()
    {
        velocity += new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized * speed * Time.deltaTime;
        velocity *= (1f - friction);
        velocity = Vector2.ClampMagnitude(velocity, MAX_SPEED);
        _rigidbody2D.MovePosition(_rigidbody2D.position + velocity);
    }
}
