using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{

    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private float shootInterval = 3f;
    [SerializeField] protected float rotationSpeed = 75f;
    [SerializeField] private GameObject sprite;
    [SerializeField] private MenuManager menuManager;
    [SerializeField] private RulesManager rulesManager;
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

    protected void Update()
    {
        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0f & RulesManager.onGame)
        {
            ShootProjectile();
            shootTimer = shootInterval;
        }
    }

    private void ShootProjectile()
    {
        Vector3 spawnPosition = transform.position;
        Projectile projectile = Instantiate(projectilePrefab, spawnPosition, sprite.transform.rotation);
        projectile.entityManager = entityManager;
    }

    protected override void Move()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        Quaternion rotation = Quaternion.Euler(0f, 0f, -horizontalInput * rotationSpeed * Time.deltaTime);
        sprite.transform.rotation *= rotation;

        // transform.Translate(Vector3.up * verticalInput * speed * Time.deltaTime);
        float verticalInput = Input.GetAxis("Vertical");
        velocity += (Vector2)(sprite.transform.rotation * transform.up) * verticalInput * speed * Time.deltaTime;
        velocity *= (1f - friction);
        velocity = Vector2.ClampMagnitude(velocity, MAX_SPEED);
        _rigidbody2D.MovePosition(_rigidbody2D.position + velocity);
    }

    public override void Die()
    {
        base.Die();
        RulesManager.onGame = false;
        SoundManager.instance.Play("Lost");
        entityManager.ScreenShake();
        menuManager.CallGameOver();
        rulesManager.GameOver();
    }
    
    public void OnCollisionEnter2D(Collision2D collision)
    {
        Entity entity = collision.transform.GetComponent<Entity>();
        if (entity != null){
            if(entityManager.scissorsPointsTo.Contains(entity.type)){ 
                print(entity.type);
                entity.Die();
            }
        }
    }
}
