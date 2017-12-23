using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using KaboomButton;
using KaboomOutput;

public class KaboomManager : Singleton<KaboomManager>
{
    private KaboomAdrioFxPlusTwoFeeders m_KaboomAdrioFxPlusTwoFeeders = null;

	public enum KaboomBoard
	{
		KaboomAdrioFxPlusTwoFeeders
	};

	public KaboomBoard m_KaboomBoard = KaboomBoard.KaboomAdrioFxPlusTwoFeeders;

    //================================================================================================//
    // Declaration of the event callbacks 

	public delegate void OnKeyEvent(KaboomButton.Button aun16Button); 

    //================================================================================================//

    private event OnKeyEvent m_OnKeyEvent;

    //================================================================================================//

    private void Awake()
    {
        m_KaboomAdrioFxPlusTwoFeeders = null;

		m_OnKeyEvent = null;

		m_KaboomAdrioFxPlusTwoFeeders = KaboomAdrioFxPlusTwoFeeders.Instance;
    }

    //================================================================================================//

    void Start()
    {
		if (m_KaboomAdrioFxPlusTwoFeeders)
        {
            m_KaboomAdrioFxPlusTwoFeeders.RegisterOnKeyboardEventCallback(KaboomOnKeyboardEvent);
        }
        else
		{
			Debug.LogError("***** Kaboom Manager is not present");
		}
    }

    //================================================================================================//

	void Update()
	{
        #if !UNITY_EDITOR
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        #endif
	}

	//================================================================================================//

	public override void OnDestroy()
    {
		if (m_KaboomAdrioFxPlusTwoFeeders)
        {
            m_KaboomAdrioFxPlusTwoFeeders.UnregisterOnKeyboardEventCallback(KaboomOnKeyboardEvent);
        }
        base.OnDestroy();
    }

	//================================================================================================//
	
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
	// OUTPUTS

    public void ClearOutput(Output i_Mask)
    {
       	if (m_KaboomAdrioFxPlusTwoFeeders)
            m_KaboomAdrioFxPlusTwoFeeders.ClearOutput(i_Mask);
    }

    //================================================================================================//

    public void SetOutput(Output i_Mask)
    {
        if (m_KaboomAdrioFxPlusTwoFeeders)
            m_KaboomAdrioFxPlusTwoFeeders.SetOutput(i_Mask);
    }

	//================================================================================================//

	public void IncrementCounter(OutputExtended i_CounterId)
	{
		if (m_KaboomAdrioFxPlusTwoFeeders)
			m_KaboomAdrioFxPlusTwoFeeders.IncrementCounter(i_CounterId);
	}

	//================================================================================================//

	public bool SetLedDisplay(UInt16 un16Value)
	{
		if (m_KaboomAdrioFxPlusTwoFeeders)
			return m_KaboomAdrioFxPlusTwoFeeders.SetLedDisplay(un16Value);

		return false;
	}

	//================================================================================================//

	private void KaboomOnKeyboardEvent(KaboomButton.Button aKey)
    {
        //Debug.Log("*****OnKeyboardEvent: " + aKey.ToString());
        if (m_OnKeyEvent != null)
			m_OnKeyEvent((KaboomButton.Button)aKey);
    }
}
