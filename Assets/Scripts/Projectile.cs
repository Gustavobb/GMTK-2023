using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float lifetime;
    [SerializeField] private GameObject sprite;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
        sprite.transform.rotation *= Quaternion.Euler(0f, 0f, 1200f * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player")) return;

        if(collision.CompareTag("Map")){
            this.gameObject.SetActive(false);
        }
        else if (collision.GetComponent<Entity>() != null){
            print("hit");
            collision.GetComponent<Entity>().Die();
            this.gameObject.SetActive(false);
        }
       
    }
}
