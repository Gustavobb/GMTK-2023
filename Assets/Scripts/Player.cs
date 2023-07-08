using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float friction = 0.1f;
    private Vector2 velocity;

    private void Update()
    {
        velocity += new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized * speed;
        velocity *= (1f - friction);
        transform.position += (Vector3)velocity * Time.deltaTime;
    }
}
