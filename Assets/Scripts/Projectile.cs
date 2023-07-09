using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float lifetime;
    [SerializeField] private GameObject sprite;
    public GameObject diePrefab;

    public EntityManager entityManager;

    private void Start()
    {
        // Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
        sprite.transform.rotation *= Quaternion.Euler(0f, 0f, 1200f * Time.deltaTime);
    }

    public void Die()
    {
        Instantiate(diePrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player")) return;

        Entity entity = collision.GetComponent<Entity>();
        if(collision.CompareTag("Map")){
            // this.gameObject.SetActive(false);
            Die();
        }
        else if (entity != null){
            if(entityManager.scissorsPointsTo.Contains(entity.type)){ 
                print(entity.type);
                entity.Die();
            }
            Die();
            //this.gameObject.SetActive(false);
        }
    }
}
