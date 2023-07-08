using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float lifetime;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
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
