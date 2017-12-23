using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArcadeInput : Singleton<ArcadeInput>
{
	public enum JoystickDirection
	{
		None = 0,
		Up,
		Down,
		Left,
		Right
	}

	private enum KaboomButtons
	{
		Left = 0,
		Select,
		Right,
		Operator
	}

	private class KaboomKeyInput
	{
		public bool m_GetKeyDown = false;
		public bool m_GetKeyUp = false;
		public bool m_GetKey = false;

		public void SetKeyDown()
		{
			m_GetKeyDown = true;
		}

		public void ResetKeyDown()
		{
			m_GetKeyDown = false;
		}

		public void SetKeyUp()
		{
			m_GetKeyUp = true;
		}

		public void ResetKeyUp()
		{
			m_GetKeyUp = false;
		}
	}

	public static bool INPUTS_LOCKED = false;

	#region Joystick
	[Range(0f, 1f)]
	[Tooltip("0 represents no dead zone and 1 represent the joystick limit as a deadzone. Default = 0.25")]
	public float m_JoystickDeadZone = 0.25f;

	private bool m_IsJoystickDown;

	private Vector3 m_InputDirection;
	private Vector3 m_TempVector;
	private Vector3 m_JoystickDownVector;

	private JoystickDirection m_DirectionCheck;
	private JoystickDirection[] m_JoysticksLastDirection = new JoystickDirection[4];
	private JoystickDirection[] m_JoysticksCurrentDirection = new JoystickDirection[4];
	#endregion

	#region GamePad Controls
	private const string m_HorizontalAxis = "Horizontal";
	private const string m_VerticalAxis = "Vertical";
	#endregion

	#region Hardware Button
	private List<KaboomKeyInput> m_CurrentlyPressedButtons = new List<KaboomKeyInput>();
	private List<KaboomKeyInput> m_CurrentlyReleasedButtons = new List<KaboomKeyInput>();
	private Dictionary<KaboomButtons, KaboomKeyInput> m_KaboomButtons = new Dictionary<KaboomButtons, KaboomKeyInput>();
	#endregion

	#region Keyboard Controls
	private const KeyCode m_Player1Up	 		= KeyCode.UpArrow;
	private const KeyCode m_Player1Right 		= KeyCode.RightArrow;
	private const KeyCode m_Player1Left	 		= KeyCode.LeftArrow;
	private const KeyCode m_Player1Down	 	= KeyCode.DownArrow;

	private const KeyCode m_Player2Up	 		= KeyCode.W;
	private const KeyCode m_Player2Right 		= KeyCode.D;
	private const KeyCode m_Player2Left	 		= KeyCode.A;
	private const KeyCode m_Player2Down	 	= KeyCode.S;

	// Operator Controls
	private const KeyCode m_OperatorUp 		= KeyCode.Keypad1;
	private const KeyCode m_OperatorDown 	= KeyCode.Keypad3;
	private const KeyCode m_SelectButton 		= 	KeyCode.Keypad2;
	private const KeyCode m_OperatorButton 	= KeyCode.O;
	#endregion

	#region MONO
	private void Start()
	{
		for(int i = 0; i < 4; i++)
		{
			m_JoysticksLastDirection[i] = JoystickDirection.None;
			m_JoysticksCurrentDirection[i] = JoystickDirection.None;
		}
		m_KaboomButtons.Add(KaboomButtons.Left, new KaboomKeyInput());
		m_KaboomButtons.Add(KaboomButtons.Select, new KaboomKeyInput());
		m_KaboomButtons.Add(KaboomButtons.Right, new KaboomKeyInput());
		m_KaboomButtons.Add(KaboomButtons.Operator, new KaboomKeyInput());

		KaboomManager.Instance.RegisterOnKeyboardEventCallback(OnKeyEvent);
	}

	private void LateUpdate()
	{
		for(int i = 0; i < GetJoysticksLength(); i++)
		{
			m_JoysticksLastDirection[i] = m_JoysticksCurrentDirection[i];
			#if !UNITY_EDITOR
			if (BFG.Instance.IsPresent((PlayerID)i))
			{
				m_JoysticksCurrentDirection[i] = GetJoystickDirectionByVector(BFG.Instance.NormalizedDirection((PlayerID)i), false);
			}
			#else
			if(i < GetJoysticksLength() )
			{
				SetControllerDirection((PlayerID)i);
				if(m_TempVector.magnitude > m_JoystickDeadZone)
				{
					m_JoysticksCurrentDirection[i] = GetJoystickDirectionByVector(m_TempVector, false);
				}
				else
				{
					m_JoysticksCurrentDirection[i] = GetKeyboardDirection((PlayerID)i);
				}
			}
			else
			{
				m_JoysticksCurrentDirection[i] = GetKeyboardDirection((PlayerID)i);
			}
			#endif
		}

		for(int i = 0; i < m_CurrentlyPressedButtons.Count; i++)
		{
			m_CurrentlyPressedButtons[i].ResetKeyDown();
			m_CurrentlyPressedButtons.RemoveAt(i);
			i--;
		}

		for(int i = 0; i < m_CurrentlyReleasedButtons.Count; i++)
		{
			m_CurrentlyReleasedButtons[i].ResetKeyUp();
			m_CurrentlyReleasedButtons.RemoveAt(i);
			i--;
		}
	}

	public override void OnDestroy()
	{
		base.OnDestroy();

		if(KaboomManager.Instance != null)
		{
			KaboomManager.Instance.UnregisterOnKeyboardEventCallback(OnKeyEvent);
		}
	}
	#endregion

	#region Getters
	public int GetJoysticksLength()
	{
		#if !UNITY_EDITOR && !UNITY_STANDALONE
		return BFG.Instance.JoysticksLength;
		#else
		return Mathf.Clamp(Input.GetJoystickNames().Length, 2, 20);
		#endif
	}
	#endregion

	#region JOYSTICK

	public Vector3 GetDirection(PlayerID i_ID = 0)
	{
		m_InputDirection = Vector3.zero;

		if (BFG.Instance.IsPresent(i_ID))
		{
			m_TempVector = BFG.Instance.NormalizedDirection(i_ID);
			if(m_TempVector.magnitude > m_JoystickDeadZone)
			{
				m_InputDirection.x = m_TempVector.x;
				m_InputDirection.y = m_TempVector.y;
			}
		}
		else
		{
			if(Input.GetKey(GetKeyCodeDirectionByID(i_ID, JoystickDirection.Left)))
			{
				m_InputDirection.x = -1f;
			}
			else if(Input.GetKey(GetKeyCodeDirectionByID(i_ID, JoystickDirection.Right)))
			{
				m_InputDirection.x = 1f;
			}

			if(Input.GetKey(GetKeyCodeDirectionByID(i_ID, JoystickDirection.Down)))
			{
				m_InputDirection.y = -1f;
			}
			else if(Input.GetKey(GetKeyCodeDirectionByID(i_ID, JoystickDirection.Up)))
			{
				m_InputDirection.y = 1f;
			}
		}
		return m_InputDirection;
	}

	public bool GetJoystickDown(JoystickDirection i_Direction, PlayerID i_ID = 0, bool i_OnAnyDirectionChange = true)
	{
		if(i_Direction == m_JoysticksCurrentDirection[(int)i_ID] && i_Direction != m_JoysticksLastDirection[(int)i_ID])
		{
			if(!i_OnAnyDirectionChange)
			{
				if(m_JoysticksLastDirection[(int)i_ID] != JoystickDirection.None)
				{
					return false;
				}
			}

			return true;
		}

		return false;
	}

	private JoystickDirection GetJoystickDirectionByVector(Vector3 i_Vector, bool i_InvertYZ = false)
	{
		if(i_Vector.magnitude > m_JoystickDeadZone)
		{
			float yAxis = i_InvertYZ == true ? i_Vector.z : i_Vector.y;

			if(Mathf.Abs(i_Vector.x) > Mathf.Abs(yAxis))
			{
				if(i_Vector.x >= 0f)
				{
					return JoystickDirection.Right;
				}
				else
				{
					return JoystickDirection.Left;
				}
			}
			else
			{
				if(yAxis >= 0f)
				{
					return JoystickDirection.Up;
				}
				else
				{
					return JoystickDirection.Down;
				}
			}
		}

		return JoystickDirection.None;
	}
	private JoystickDirection GetKeyboardDirection(PlayerID i_ID)
	{
		JoystickDirection direction = JoystickDirection.None;
		if(Input.GetKey(GetKeyCodeDirectionByID(i_ID, JoystickDirection.Left)))
		{
			direction = JoystickDirection.Left;
		}
		else if(Input.GetKey(GetKeyCodeDirectionByID(i_ID, JoystickDirection.Right)))
		{
			direction = JoystickDirection.Right;
		}
		else if(Input.GetKey(GetKeyCodeDirectionByID(i_ID, JoystickDirection.Down)))
		{
			direction = JoystickDirection.Down;
		}
		else if(Input.GetKey(GetKeyCodeDirectionByID(i_ID, JoystickDirection.Up)))
		{
			direction = JoystickDirection.Up;
		}
		return direction;
	}

	private void SetControllerDirection(PlayerID i_ID)
	{
		//m_TempVector.x = Input.GetAxis(m_HorizontalAxis+((int)i_ID).ToString());
		//m_TempVector.z = Input.GetAxis(m_VerticalAxis+((int)i_ID).ToString());
		//m_TempVector.y = 0;
	}

	public Vector3 GetControllerDirection(PlayerID i_ID)
	{
		SetControllerDirection(i_ID);
		Debug.Log("GetControllerDirection " + i_ID.ToString() + " : " + m_TempVector);
		return m_TempVector;
	}

	private KeyCode GetKeyCodeDirectionByID(PlayerID i_ID, JoystickDirection i_Direction)
	{
		switch(i_ID)
		{
		case PlayerID.One:
			switch(i_Direction)
			{
			case JoystickDirection.Up:
				return m_Player1Up;
			case JoystickDirection.Right:
				return m_Player1Right;
			case JoystickDirection.Left:
				return m_Player1Left;
			case JoystickDirection.Down:
				return m_Player1Down;
			}
			break;
		case PlayerID.Two:
			switch(i_Direction)
			{
			case JoystickDirection.Up:
				return m_Player2Up;
			case JoystickDirection.Right:
				return m_Player2Right;
			case JoystickDirection.Left:
				return m_Player2Left;
			case JoystickDirection.Down:
				return m_Player2Down;
			}
			break;
		}

		return m_Player1Up;
	}
	#endregion

	#region BOARD CALLBACKS
	private void SetOnKeyPressed(KaboomKeyInput i_KeyInput)
	{
		i_KeyInput.SetKeyDown();
		m_CurrentlyPressedButtons.Add(i_KeyInput);
	}

	private void SetOnKeyReleased(KaboomKeyInput i_KeyInput)
	{
		i_KeyInput.SetKeyUp();
		m_CurrentlyReleasedButtons.Add(i_KeyInput);
	}

	private void OnKeyEvent(KaboomButton.Button aun16Button)
	{
		switch(aun16Button)
		{
		case KaboomButton.Button.BUTTON_LEFT_PRESS:
			m_KaboomButtons[KaboomButtons.Left].m_GetKey = true;
			SetOnKeyPressed(m_KaboomButtons[KaboomButtons.Left]);
			break;
		case KaboomButton.Button.BUTTON_LEFT_RELEASE:
			m_KaboomButtons[KaboomButtons.Left].m_GetKey = false;
			SetOnKeyReleased(m_KaboomButtons[KaboomButtons.Left]);
			break;
		case KaboomButton.Button.BUTTON_SELECT_PRESS:
			m_KaboomButtons[KaboomButtons.Select].m_GetKey = true;
			SetOnKeyPressed(m_KaboomButtons[KaboomButtons.Select]);
			break;
		case KaboomButton.Button.BUTTON_SELECT_RELEASE:
			m_KaboomButtons[KaboomButtons.Select].m_GetKey = false;
			SetOnKeyReleased(m_KaboomButtons[KaboomButtons.Select]);
			break;
		case KaboomButton.Button.BUTTON_RIGHT_PRESS:
			m_KaboomButtons[KaboomButtons.Right].m_GetKey = true;
			SetOnKeyPressed(m_KaboomButtons[KaboomButtons.Right]);
			break;
		case KaboomButton.Button.BUTTON_RIGHT_RELEASE:
			m_KaboomButtons[KaboomButtons.Right].m_GetKey = false;
			SetOnKeyReleased(m_KaboomButtons[KaboomButtons.Right]);
			break;
		case KaboomButton.Button.BUTTON_OPERATOR_PRESS:
			m_KaboomButtons[KaboomButtons.Operator].m_GetKey = true;
			SetOnKeyPressed(m_KaboomButtons[KaboomButtons.Operator]);
			break;
		case KaboomButton.Button.BUTTON_OPERATOR_RELEASE:
			m_KaboomButtons[KaboomButtons.Operator].m_GetKey = false;
			SetOnKeyReleased(m_KaboomButtons[KaboomButtons.Operator]);
			break;
		}
	}
	#endregion

	#region KABOOM KEYDOWN
	public bool GetP2KeyDown()
	{
		return GetRightKeyDown();
	}
	public bool GetP1KeyUp()
	{
		return GetLeftKeyUp();
	}
	public bool GetP2KeyUp()
	{
		return GetRightKeyUp();
	}
	public bool GetP1OrP2KeyDown()
	{
		return GetLeftKeyDown() || GetRightKeyDown();
	}
	public bool GetSelectKeyDown()
	{
		return m_KaboomButtons[KaboomButtons.Select].m_GetKeyDown || Input.GetKeyDown(m_SelectButton);
	}
	private bool GetLeftKeyDown()
	{
		return m_KaboomButtons[KaboomButtons.Left].m_GetKeyDown || Input.GetKeyDown(m_Player1Up) || Input.GetKeyDown(m_OperatorUp);
	}
	private bool GetRightKeyDown()
	{
		return m_KaboomButtons[KaboomButtons.Right].m_GetKeyDown || Input.GetKeyDown(m_Player2Up) || Input.GetKeyDown(m_OperatorDown);
	}
	private bool GetLeftKeyUp()
	{
		return m_KaboomButtons[KaboomButtons.Left].m_GetKeyUp || Input.GetKeyUp(m_Player1Up) || Input.GetKeyUp(m_OperatorUp);
	}
	private bool GetRightKeyUp()
	{
		return m_KaboomButtons[KaboomButtons.Right].m_GetKeyUp || Input.GetKeyUp(m_Player2Up) || Input.GetKeyUp(m_OperatorDown);
	}
	public bool GetP1KeyDown()
	{
		return GetLeftKeyDown();
	}
   	#endregion
}
