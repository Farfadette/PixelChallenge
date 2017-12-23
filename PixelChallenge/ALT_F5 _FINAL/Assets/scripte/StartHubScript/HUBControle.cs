using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HUBControle : MonoBehaviour 
{
	public Button StartGame;
	public Button ExitGame;


	void Start () 
	{
		StartGame.GetComponent<Button> ();
		ExitGame.GetComponents<Button> ();

	}

	public void FristHub ()
	{
		StartGame.enabled = true;
		ExitGame.enabled = true;

	}
	public void StartLevel()
	{
		Application.LoadLevel ("ALT_F5");
	}
	public void Leave()
	{
		Application.Quit();
	}

	void Update () 
	{
		
	}
}
