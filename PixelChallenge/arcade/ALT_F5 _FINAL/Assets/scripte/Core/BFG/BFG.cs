using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Linq;
using System.Text;


/// <summary>
/// The desktop implementation of FingerGestures, using mouse device input
/// </summary>
public class BFG : Singleton<BFG>
{

	//================================================================================================//

	[StructLayout(LayoutKind.Sequential)]
	internal struct XINPUT_STATE
	{
		[MarshalAs(UnmanagedType.U4)]
		public uint dwPacketNumber;
		[MarshalAs(UnmanagedType.U2)]
		public UInt16 wButtons;
		[MarshalAs(UnmanagedType.U1)]
		public byte bLeftTrigger;
		[MarshalAs(UnmanagedType.U1)]
		public byte bRightTrigger;
		[MarshalAs(UnmanagedType.I2)]
		public Int16 sThumbLX;
		[MarshalAs(UnmanagedType.I2)]
		public Int16 sThumbLY;
		[MarshalAs(UnmanagedType.I2)]
		public Int16 sThumbRX;
		[MarshalAs(UnmanagedType.I2)]
		public Int16 sThumbRY;
	}

	[DllImport("xinput1_3")]
	extern static int XInputGetState(uint index, ref XINPUT_STATE pState);

	//================================================================================================//

	private const bool mb_UseSmallRange = false;

	public bool[] mb_Ports;

	public bool InvertX = false;
	public bool InvertY = false;

	#pragma warning disable 0414 // Ignore not used warning
	private XINPUT_STATE pState;
	#pragma warning restore 0414

	private const int BFG_NB_BUTTONS = 3;
	
	#if !UNITY_EDITOR
	private int mi_Range = 65536;
	private int mi_HalfRange = 32768;
	#endif

	private class Joystick
	{
		public bool bPresent;
		public bool[] bButtonsState;
		public bool[] bButtonsPreviousState;
		public Vector3 vPosition;
		public Vector3 vPreviousPosition;
		public Vector3 vNormalizedDirection;
	}

	private Joystick[] m_Joystick;

	//================================================================================================//

    void Start()
    {
		pState = new XINPUT_STATE();
		m_Joystick = new Joystick[mb_Ports.Length];

		for (int i = 0; i < m_Joystick.Length; i++)
		{
			m_Joystick[i] = new Joystick();
			m_Joystick[i].bPresent = false;
			m_Joystick[i].vPosition = Vector3.zero;
			m_Joystick[i].vPreviousPosition = Vector3.zero;
			m_Joystick[i].vNormalizedDirection = Vector3.zero;
			m_Joystick[i].bButtonsState = new bool[BFG_NB_BUTTONS];
			m_Joystick[i].bButtonsPreviousState = new bool[BFG_NB_BUTTONS];
			for (int j = 0; j < BFG_NB_BUTTONS; j++)
			{
				m_Joystick[i].bButtonsState[j] = false;
				m_Joystick[i].bButtonsPreviousState[j] = false;
			}
		}

		#if !UNITY_EDITOR
		if(mb_UseSmallRange)
		{
			mi_Range = 256;
			mi_HalfRange = 128;
		}
		else
		{
			mi_Range = 65536;
			mi_HalfRange = 32768;
		}
		#endif
	}

	//================================================================================================//
	
	void Update()
    {
		#if !UNITY_EDITOR
		for (uint i = 0; i < mb_Ports.Length; i++)
		{
			if (mb_Ports[i])
			{
				m_Joystick[i].vNormalizedDirection = Vector3.zero;
				int result = XInputGetState(i, ref pState);
				//Debug.Log("===== " + i.ToString() + " : " + result.ToString());
				if (result >= 0)
				{
					m_Joystick[i].bPresent = true;
					//Debug.Log("***** Button: " + pState.wButtons.ToString());
					m_Joystick[i].bButtonsPreviousState[0] = m_Joystick[i].bButtonsState[0];
					if ((pState.wButtons & 0x8000) == 0x8000)
						m_Joystick[i].bButtonsState[0] = true;
					else
						m_Joystick[i].bButtonsState[0] = false;

					m_Joystick[i].bButtonsPreviousState[1] = m_Joystick[i].bButtonsState[1];
					if ((pState.wButtons & 0x1000) == 0x1000)
						m_Joystick[i].bButtonsState[1] = true;
					else
						m_Joystick[i].bButtonsState[1] = false;

					m_Joystick[i].vPreviousPosition = m_Joystick[i].vPosition;
					float posx = pState.sThumbLX + mi_HalfRange;
					if(InvertX)
						posx = mi_Range - posx;

					#if USE_TRIGGER
					posx -= mi_HalfRange;
					#endif

					int tempPosX = (int)(posx * Screen.width) / mi_Range;
					m_Joystick[i].vPosition.x = (posx > 10000 || posx < -10000) ? tempPosX : 0f;

					float posy = pState.sThumbLY + mi_HalfRange;
					if (InvertY)
						posy = mi_Range - posy;

					#if USE_TRIGGER
					posy -= mi_HalfRange;
					#endif

					m_Joystick[i].vPosition.y = 0;
					int tempPosY = (int)(posy * Screen.height) / mi_Range;
					m_Joystick[i].vPosition.z = (posy > 10000 || posy < -10000) ? tempPosY : 0f;

					m_Joystick[i].vNormalizedDirection.x = pState.sThumbLX/(float)mi_HalfRange;
					m_Joystick[i].vNormalizedDirection.y = pState.sThumbLY/(float)mi_HalfRange;
				}
			}
		}
	#endif

	}

	//======================================================================================================//

	public Vector3 mousePosition(int id)
	{
		if(id < m_Joystick.Length)
		{
			if(m_Joystick[id].bPresent)
				return m_Joystick[id].vPosition;
		}
		return Input.mousePosition;
	}

	//======================================================================================================//
	
	public Vector3 NormalizedDirection(PlayerID id)
	{
		int index = (int)id;
		if(index < m_Joystick.Length)
		{
			if(m_Joystick[index].bPresent)
			{
				return m_Joystick[index].vNormalizedDirection;
			}
		}
		return Input.mousePosition.normalized;
	}

	//======================================================================================================//

	public Vector3 DeltaMousePosition(int id)
	{
		if(id < m_Joystick.Length)
		{
			if(m_Joystick[id].bPresent)
			{
				Vector3 deltaPos = (m_Joystick[id].vPosition - m_Joystick[id].vPreviousPosition) * Time.deltaTime;
				m_Joystick[id].vPreviousPosition = m_Joystick[id].vPosition;
				return deltaPos;
			}
		}
	return Input.mousePosition;
	}

	//======================================================================================================//
	public bool GetMouseButton(int id, int iButton)
	{
		if (id < m_Joystick.Length)
		{
			if (m_Joystick[id].bPresent)
				return m_Joystick[id].bButtonsState[iButton];
		}
		return Input.GetMouseButton(iButton);
	}

	//======================================================================================================//

	public bool GetMouseButtonUp(int id, int iButton)
	{
		if (id < m_Joystick.Length)
		{
			if (m_Joystick[id].bPresent)
			{
				if (m_Joystick[id].bButtonsPreviousState[iButton] == true && m_Joystick[id].bButtonsState[iButton] == false)
					return true;
				else
					return false;
			}
		}
		
		return Input.GetMouseButtonUp(iButton);
	}

	//======================================================================================================//

	public bool GetMouseButtonDown(int id, int iButton)
	{
		if (id < m_Joystick.Length)
		{
			if (m_Joystick[id].bPresent)
			{
				if (m_Joystick[id].bButtonsPreviousState[iButton] == false && m_Joystick[id].bButtonsState[iButton] == true)
					return true;
				else
					return false;
			}
		}
		
		return Input.GetMouseButtonUp(iButton);
	}

	//======================================================================================================//

	public bool IsPresent(PlayerID id)
	{
		int index = (int)id;
		if (index < m_Joystick.Length)
			return m_Joystick[index].bPresent;

		return false;
	}
}


