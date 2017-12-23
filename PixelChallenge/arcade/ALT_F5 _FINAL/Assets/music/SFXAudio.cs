using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXAudio : MonoBehaviour
{
    public AudioSource m_Source;

    private float m_ClipLength;
    private float m_Counter;

    public void Setup(AudioClip aClip, Vector3 aPos)
    {
        m_ClipLength = aClip.length;
        transform.position = aPos;

        m_Source.clip = aClip;
        m_Source.Play();
    }

    public void Update()
    {
        m_Counter += Time.deltaTime;

        if (m_Counter >= m_ClipLength)
        {
            Destroy(gameObject);
        }
    }
}
