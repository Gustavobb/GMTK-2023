using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadow : MonoBehaviour
{
    [SerializeField] private float sunRotation = 0f;
    [SerializeField] private Vector2 shadowOffset = new Vector2(0f, -1f);

    private void Update()
    {
        // float2 shadowDir = Rotate(float2(0, -_ShadowOffset), _SunRotation);
        Quaternion rotation = Quaternion.Euler(0f, 0f, sunRotation);
        Vector2 shadowDir = rotation * shadowOffset;
        transform.position = (Vector2)transform.parent.position + shadowDir * 0.5f;
    }
}
