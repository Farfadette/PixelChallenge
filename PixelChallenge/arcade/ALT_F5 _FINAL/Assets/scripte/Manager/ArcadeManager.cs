
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public enum PlayerID 
{
	One = 0,
	Two = 1,
	Count
}

public class ArcadeManager : Singleton<ArcadeManager> 
{
	[System.Serializable]
	public class KaboomData
	{
		public PlayerID m_PlayerID;
		public KaboomOutput.Output m_LightOutput;
	}

	private KaboomData KaboomDataByPlayerID(PlayerID i_ID)
	{
		for(int i = 0; i < m_KaboomData.Count; i++)
		{
			if(m_KaboomData[i].m_PlayerID == i_ID)
			{
				return m_KaboomData[i];
			}
		}

		return null;
	}

	private const float LIGHT_INTERVAL = 0.5f;

	public List<KaboomData> m_KaboomData = new List<KaboomData>();
	private bool[] m_PlayerIsPlaying = new bool[2];
	private Coroutine[] m_FlashLightCoroutines = new Coroutine[2];

	#region PLAYER
	public bool IsPlayerPlaying(PlayerID i_ID)
	{
		return m_PlayerIsPlaying[(int)i_ID];
	}

	public bool BothPlayerPlaying()
	{
		return m_PlayerIsPlaying[0] && m_PlayerIsPlaying[1];
	}

	public void SetPlayerReady(PlayerID i_ID)
	{
		m_PlayerIsPlaying[(int)i_ID] = true;
	}
		
	public void CheckIfPlayerIsReady()
	{
	}
	public void ResetPlayers()
	{
		m_PlayerIsPlaying[0] = false;
		m_PlayerIsPlaying[1] = false;
	}
	#endregion

	#region OUTPUTS
	public void ClearOutput(KaboomOutput.Output i_Mask)
	{
		KaboomManager.Instance.ClearOutput(i_Mask);
	}
	public void SetOutput(KaboomOutput.Output i_Mask)
	{
		KaboomManager.Instance.SetOutput(i_Mask);
	}
	#endregion

	#region Lights
	public void ClearPlayerLight(PlayerID i_ID)
	{
		if(m_FlashLightCoroutines[(int)i_ID] != null)
		{
			StopCoroutine(m_FlashLightCoroutines[(int)i_ID]);
			m_FlashLightCoroutines[(int)i_ID] = null;
		}

		ClearOutput(KaboomDataByPlayerID(i_ID).m_LightOutput);
	}

	public void OpenPlayerLight(PlayerID i_ID, bool i_Flash)
	{
		if(i_Flash)
		{
			if(m_FlashLightCoroutines[(int)i_ID] == null)
			{
				m_FlashLightCoroutines[(int)i_ID] = StartCoroutine(FlashLight(KaboomDataByPlayerID(i_ID).m_LightOutput, LIGHT_INTERVAL));
			}
		}
		else
		{
			SetOutput(KaboomDataByPlayerID(i_ID).m_LightOutput);
		}
	}

	public void OpenAllPlayerLight(bool i_Flash)
	{
		for(int i = 0; i < m_KaboomData.Count; i++)
		{
			OpenPlayerLight(m_KaboomData[i].m_PlayerID, i_Flash);
		}
	}

	private IEnumerator FlashLight(KaboomOutput.Output i_Output, float i_Delay)
	{
		while(true)
		{
			yield return new WaitForSeconds(i_Delay);
			SetOutput(i_Output);
			yield return new WaitForSeconds(i_Delay);
			ClearOutput(i_Output);
	
			yield return null;
		}
	}
	#endregion
}
