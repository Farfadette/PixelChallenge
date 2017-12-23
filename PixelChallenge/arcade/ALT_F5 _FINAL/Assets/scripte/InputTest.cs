using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is only for test purposes. Should'nt be used at all and should be deleted
public class InputTest : MonoBehaviour 
{
	public UnityEngine.UI.Text m_Text;
	public Transform m_JoystickTextTrP1;
	public Transform m_JoystickTextTrP2;

	// When getting inputs from ArcadeInput, it needs to be from the LateUpdate 
	private void LateUpdate () 
	{
		if(ArcadeInput.Instance.GetP1KeyDown())
		{
			if(m_Text != null)
			{
				m_Text.text = "P1 Button Pressed";
			}
		}
		else if(ArcadeInput.Instance.GetP1KeyUp())
		{
			if(m_Text != null)
			{
				m_Text.text = "P1 Button Released";
			}
		}

		if(ArcadeInput.Instance.GetP2KeyDown())
		{
			if(m_Text != null)
			{
				m_Text.text = "P2 Button Pressed";
			}
		}
		else if(ArcadeInput.Instance.GetP2KeyUp())
		{
			if(m_Text != null)
			{
				m_Text.text = "P2 Button Released";
			}
		}

		m_JoystickTextTrP1.position += ArcadeInput.Instance.GetDirection(PlayerID.One) * (50f * Time.deltaTime);
		m_JoystickTextTrP2.position += ArcadeInput.Instance.GetDirection(PlayerID.Two) * (50f * Time.deltaTime);

		//Debug.Log(ArcadeInput.Instance.GetDirection(PlayerID.One));
		//Debug.Log(ArcadeInput.Instance.GetJoystickDown(ArcadeInput.JoystickDirection.Up, PlayerID.One));
	}
}
