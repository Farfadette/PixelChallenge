using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deadZoneSpawner : MonoBehaviour
{
    public GameObject blackHole;
    private float rotationSpeed = -5f;
    Rigidbody2D rigidBody;
    Light blackHoleLight;
    Vector3 blackHoleSize = new Vector3(1, 1, 0);

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        blackHoleLight = blackHole.GetComponent<Light>();
        blackHoleLight.range = 0f;
        transform.localScale = Vector2.zero;
    }

    void Update()
    {
        if (blackHole.transform.localScale.x < 1f)
        {
            blackHole.transform.localScale = new Vector3(blackHole.transform.localScale.x + 0.75f * Time.deltaTime, blackHole.transform.localScale.y + 0.75f * Time.deltaTime, 0);
            blackHoleLight.range += 2.25f * Time.deltaTime;
        }
        rigidBody.rotation = rigidBody.rotation + rotationSpeed;
    }
}
