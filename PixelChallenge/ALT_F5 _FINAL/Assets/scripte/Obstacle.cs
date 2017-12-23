using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{


    private float time = 2.5f;

	// Use this for initialization
	void Start ()
    {
        Invoke("DestroySelf", time);
	}
	
    void DestroySelf()
    {
        DestroyObject(gameObject);
    }
}
