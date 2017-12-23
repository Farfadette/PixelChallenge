using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using KaboomButton;
using KaboomOutput;
using KaboomAdrioFxPlusTwoFeedersDllWrapper;

// Type Aliases:
using KABOOM_DLL_ERROR = KaboomAdrioFxPlusTwoFeedersDllWrapper.KaboomAdrioFxPlusTwoFeedersDll.KABOOM_DLL_ERROR;
using KABOOM_WHEEL_SPINNER_PARAMS = KaboomAdrioFxPlusTwoFeedersDllWrapper.KaboomAdrioFxPlusTwoFeedersDll.KaboomWheelSpinnerParams;

// This manager goes with the board IO AdrioFX+
public class KaboomAdrioFxPlusTwoFeeders : Singleton<KaboomAdrioFxPlusTwoFeeders>
{

    private enum AdrioFxPlusButton
    {
        BUTTON_NONE = 0,
        BUTTON_OPERATOR = 1,
        BUTTON_DOWN = 2,
        BUTTON_UP = 4,
        BUTTON_SELECT = 8
    }

    private class KeyPadEvent
    {
        public ushort KeyPressed;
        public KeyPadEvent(ushort k)
        {
            KeyPressed = k;
        }
    }

    //================================================================================================//
    // Declaration of the event callbacks 

	public delegate void OnKeyEvent(KaboomButton.Button aun16Button);

    //================================================================================================//

    private event OnKeyEvent m_OnKeyEvent;

    private KaboomAdrioFxPlusTwoFeedersDll.FuncKaboomKeyboardEvent kaboomKeyPressEventDelegate = new KaboomAdrioFxPlusTwoFeedersDll.FuncKaboomKeyboardEvent(OnKeyboardEvent);
  
    //================================================================================================//

    private bool mb_Ready;

    private int mi_SerialPort;
    private const int SERIAL_PORT_MIN = 2;
	private const int SERIAL_PORT_MAX = 19;

    private static int m_ButtonMask;

	private static List<KaboomButton.Button> m_ButtonEvents = new List<KaboomButton.Button>();

	private float m_PrizeTime;
	private bool mb_WaitForPrize;

	//================================================================================================//
    // Use this for initialization

    private void Awake()
	{
        mb_Ready = false;

        m_ButtonMask = 48;

        _Initialize();
	}

	//================================================================================================//

    private void _Initialize()
	{
        KABOOM_DLL_ERROR vError = KABOOM_DLL_ERROR.NO_ERROR;

        if (mb_Ready == false)
		{
			if (Application.platform == RuntimePlatform.LinuxPlayer)
				mi_SerialPort = -1;
			else
				mi_SerialPort = SERIAL_PORT_MIN;

			do
			{
				vError = KaboomAdrioFxPlusTwoFeedersDll.InitLib(++mi_SerialPort,
												kaboomKeyPressEventDelegate,
												null,
												null,
												null,
												null);

				Debug.Log("****Kaboom InitLib2Feeder To Serial : " + mi_SerialPort.ToString() + " " + vError.ToString());
				if (vError == KABOOM_DLL_ERROR.NO_ERROR)
				{
					mb_Ready = true;
				}
            } while (mi_SerialPort != SERIAL_PORT_MAX && vError != KABOOM_DLL_ERROR.NO_ERROR);
		}
	}

    //================================================================================================//

    public override void OnDestroy()
    {
        CloseLib();
        base.OnDestroy();
	}

    //================================================================================================//
    
    void OnApplicationQuit() 
	{
        CloseLib();
	}

    //================================================================================================//

    void CloseLib()
    {
        if (mb_Ready)
        {
			KaboomAdrioFxPlusTwoFeedersDll.ClearOutput((UInt16)Output.OUTPUT_ALL);
            KaboomAdrioFxPlusTwoFeedersDll.CloseLib();
            mb_Ready = false;
        }
    }

    //================================================================================================//

    void Update()
    {
		if (mb_Ready)
		{
			// Buttons
			if (m_OnKeyEvent != null)
			{
				for (int i = 0; i < m_ButtonEvents.Count; i++)
				{
					m_OnKeyEvent(m_ButtonEvents[i]);
				}
			}
			m_ButtonEvents.Clear();
		}
	}

    //================================================================================================//
    // EVENTS

    public void RegisterOnKeyboardEventCallback(OnKeyEvent ao_Event)
    {
        m_OnKeyEvent += ao_Event;
    }

    //================================================================================================//

    public void UnregisterOnKeyboardEventCallback(OnKeyEvent ao_Event)
    {
        m_OnKeyEvent -= ao_Event;
    }

    //================================================================================================//
    // BUTTONS

    private static void OnKeyboardEvent(UInt16 aun16Button)
	{
        int iKey = (int)aun16Button;
        int iDiff = iKey ^ m_ButtonMask;

		if ((iDiff & (int)AdrioFxPlusButton.BUTTON_OPERATOR) != 0)
        {
            if ((iDiff & iKey) != 0)
				m_ButtonEvents.Add(KaboomButton.Button.BUTTON_OPERATOR_PRESS);
            else
				m_ButtonEvents.Add(KaboomButton.Button.BUTTON_OPERATOR_RELEASE);
        }

		if ((iDiff & (int)AdrioFxPlusButton.BUTTON_DOWN) != 0)
        {
            if ((iDiff & iKey) != 0)
				m_ButtonEvents.Add(KaboomButton.Button.BUTTON_DOWN_PRESS);
            else
				m_ButtonEvents.Add(KaboomButton.Button.BUTTON_DOWN_RELEASE);
        }

		if ((iDiff & (int)AdrioFxPlusButton.BUTTON_UP) != 0)
        {
            if ((iDiff & iKey) != 0)
				m_ButtonEvents.Add(KaboomButton.Button.BUTTON_UP_PRESS);
            else
				m_ButtonEvents.Add(KaboomButton.Button.BUTTON_UP_RELEASE);
        }

		if ((iDiff & (int)AdrioFxPlusButton.BUTTON_SELECT) != 0)
        {
            if ((iDiff & iKey) != 0)
				m_ButtonEvents.Add(KaboomButton.Button.BUTTON_SELECT_PRESS);
            else
				m_ButtonEvents.Add(KaboomButton.Button.BUTTON_SELECT_RELEASE);
        }

        m_ButtonMask = aun16Button;
	}

	//================================================================================================//
    // OUTPUTS

    public void ClearOutput(Output i_Mask)
    {
        //Debug.Log("***** ClearOutput:" + i_Mask);
        if (mb_Ready)
        {
            KABOOM_DLL_ERROR vError = KaboomAdrioFxPlusTwoFeedersDll.ClearOutput((UInt16)i_Mask);
            if (vError != KABOOM_DLL_ERROR.NO_ERROR)
                Debug.Log("***** ClearOutput Error:" + vError.ToString());
        }
    }

    //================================================================================================//

    public void SetOutput(Output i_Mask)
    {
        //Debug.Log("***** SetOutput:" + i_Mask);
        if (mb_Ready)
        {
            KABOOM_DLL_ERROR vError = KaboomAdrioFxPlusTwoFeedersDll.SetOutput((UInt16)i_Mask);
            if (vError != KABOOM_DLL_ERROR.NO_ERROR)
                Debug.Log("***** SetOutput Error:" + vError.ToString());
        }
    }

	//================================================================================================//

	public void IncrementCounter(OutputExtended i_CounterId)
	{
		StartCoroutine(IncrementMechanicalCounter(i_CounterId));
	}

	//================================================================================================//

	private IEnumerator IncrementMechanicalCounter(OutputExtended i_CounterId)
	{
		KaboomAdrioFxPlusTwoFeedersDll.SetExtendedOutput((UInt16)i_CounterId);

		yield return new WaitForSeconds(1.0f);

		KaboomAdrioFxPlusTwoFeedersDll.ClearExtendedOutput((UInt16)i_CounterId);
	}
		
	//================================================================================================//
	// LED DISPLAY

	public bool SetLedDisplay(UInt16 un16Value)
	{
		bool b_Ret = false;
		if (mb_Ready)
		{
			KABOOM_DLL_ERROR vError = KaboomAdrioFxPlusTwoFeedersDll.SetLedDisplay(un16Value);
			if (vError == KABOOM_DLL_ERROR.NO_ERROR)
				b_Ret = true;
		}
		return b_Ret;
	}
}