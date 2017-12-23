using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager m_Instance;

    public static AudioManager Instance
    {
        get { return m_Instance; }
    }

    public GameObject m_SFXAudio;

    public AudioSource m_MusicSource;

    public AudioClip m_Music1;
    public AudioClip m_Music2;

    public AudioClip m_SFX1;
    public AudioClip m_SFX2;

    private void Awake()
    {
        if (m_Instance == null)
        {
            m_Instance = this;
        }
        else
        {
            Debug.LogWarning("Attention, il y a deux AudioManager");
            Destroy(this.gameObject);
        }

        PlayMusic(m_Music1);
    }

    public void PlayMusic(AudioClip aClip)
    {
        m_MusicSource.clip = aClip;
        m_MusicSource.Play();
    }

    public void PlaySFX(AudioClip aClip, Vector2 aPos)
    {
        GameObject go = Instantiate(m_SFXAudio);
        go.GetComponent<SFXAudio>().Setup(aClip, aPos);
    }

    public void OnGUI() {

        if (GUI.Button(new Rect(0, 0, 100, 20), "PlaySFX1"))
        {
            PlaySFX(m_SFX1, new Vector2(-5, 0));
        }

        if (GUI.Button(new Rect(0, 25, 100, 20), "PlaySFX2"))
        {
            PlaySFX(m_SFX2, new Vector2(5, 0));
        }
        
        if (GUI.Button(new Rect(0, 50, 100, 20), "SwitchMusic1"))
        {
            SwitchMusic(m_Music1, 0.5f);
        }

        if (GUI.Button(new Rect(0, 75, 100, 20), "SwitchMusic2"))
        {
            SwitchMusic(m_Music2, 2f);
        }
    }

    public void Update()
    {
        if (m_Fading)
        {
            if (!m_IsFadeIn)
            {
                FadeOut();
            }
            else
            {
                FadeIn();
            }
        }
    }

    private bool m_Fading = false;
    private bool m_IsFadeIn = false;
    private AudioClip m_NextClip;
    private float m_Duration;

    public void SwitchMusic(AudioClip aNextClip, float aDuration)
    {
        m_NextClip = aNextClip;
        m_Duration = aDuration;

        m_Fading = true;
        m_IsFadeIn = false;
    }

    private void FadeOut()
    {
        m_MusicSource.volume -= Time.deltaTime / m_Duration;

        if (m_MusicSource.volume <= 0)
        {
            m_IsFadeIn = true;

            m_MusicSource.clip = m_NextClip;
            m_MusicSource.Play();
        }
    }

    private void FadeIn()
    {
        m_MusicSource.volume += Time.deltaTime / m_Duration;

        if (m_MusicSource.volume >= 1)
        {
            m_Fading = false;
        }
    }
}
