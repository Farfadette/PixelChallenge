using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour 
{

	private Vector3 PosInit = Vector3.zero;
	private Vector3 PosShake;
	private float ShakeY;
	private float ShakeX;
	private float time;
	private bool isShaking;
	private int timeCalled = 0;



	void Update()
	{
		ShakeY = Random.Range (-0.3f, 0.3f);
		ShakeX = Random.Range (-0.3f, 0.3f);

		time = Time.time + 1f;

		if (isShaking == true) 
		{
			PosShake = new Vector3 (ShakeX, ShakeY, -10.0f);
			transform.position = PosShake;
			timeCalled++;
			if (timeCalled == 20) 
			{
				isShaking = false;
				timeCalled = 0 ;
				ResetCam ();
			}

		}


		/*if (Input.GetKey(KeyCode.Space)) 
		{
			

			ShakeIt ();

		} else {

			ResetCam ();

		}*/
	}
	public void ShakeIt()
	{
		isShaking = true;
			/*Debug.Log("scripting") ;
			if (Time.time >= 0.1f) 
			{
			Debug.Log ("avant for");
				

				Debug.Log ("dans for");
					PosShake = new Vector3 (ShakeX, ShakeY, -10.0f);
					transform.position = PosShake;
				
			}
		ResetCam ();*/

	}

	private void ResetCam()
	{
		transform.position = new Vector3 (0, 0, -10.0f);
	}




}
