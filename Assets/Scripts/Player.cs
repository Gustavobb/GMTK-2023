using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    [SerializeField] private EntityManager.Type type;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float shootInterval = 3f;
    [SerializeField] protected float rotationSpeed = 75f; 
    private float shootTimer;


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

    protected override void Update()
    {
        base.Update();

        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0f)
        {
            ShootProjectile();
            shootTimer = shootInterval;
        }
    }

    private void ShootProjectile()
    {
        Vector3 spawnPosition = transform.position + transform.up;

        GameObject projectile = Instantiate(projectilePrefab, spawnPosition, transform.rotation);

    }

    protected override void Move()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.forward * -horizontalInput * rotationSpeed * Time.deltaTime);

        // transform.Translate(Vector3.up * verticalInput * speed * Time.deltaTime);
        float verticalInput = Input.GetAxis("Vertical");
        velocity += (Vector2)transform.up * verticalInput * speed * Time.deltaTime;
        velocity *= (1f - friction);
        velocity = Vector2.ClampMagnitude(velocity, MAX_SPEED);
        _rigidbody2D.MovePosition(_rigidbody2D.position + velocity);
    }
}
