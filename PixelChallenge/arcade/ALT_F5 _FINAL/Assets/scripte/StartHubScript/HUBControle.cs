using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HUBControle : MonoBehaviour 
{
	public Button StartGame;
	public Button ExitGame;
    private bool Gstart = false;


	void Start () 
	{
		StartGame.GetComponent<Button> ();


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

	void LateUpdate () 
	{

        if (ArcadeInput.Instance.GetSelectKeyDown() && !Gstart)
        {
            Gstart = true;
            Application.LoadLevel("ALT_F5");
        }
    }
}
