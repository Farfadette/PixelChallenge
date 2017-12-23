using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBrightness : MonoBehaviour {

	public float brightness;

	private SpriteRenderer renderer;


	// Use this for initialization
	void Start () 
	{
		renderer = GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		Color color = renderer.color;

		color.a = brightness;

		renderer.color = color;
	}
}
