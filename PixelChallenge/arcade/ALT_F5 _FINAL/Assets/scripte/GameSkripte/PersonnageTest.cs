using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonnageTest : MonoBehaviour {

	public float speed = 3f;
	private Rigidbody2D rgd2;


	// Use this for initialization
	void Start () 
	{
		rgd2 = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		float x = Input.GetAxisRaw ("Horizontal");
		float y = Input.GetAxisRaw ("Vertical");


		Vector2 movement = new Vector2 (x, y) * speed;

		rgd2.velocity = movement;

	}
}
