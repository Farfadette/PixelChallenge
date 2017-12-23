using UnityEngine;

public class ApplicationManager : Singleton<ApplicationManager>
{
	public enum ScreenResolution
	{
		Landscape,
		Portrait
	}

	public ScreenResolution m_ScreenResolution;
    public bool m_IsStandAlone = false;

	private void Awake()
    {
        // Set target framerate
        DontDestroyOnLoad(this);
#if UNITY_EDITOR
        Application.targetFrameRate = 60;
		#else
		Cursor.visible = false;
		#endif
    }

    public void Start()
	{
		int width = 0;
		int height = 0;
		GetResolution(ref width, ref height);

		if(m_IsStandAlone)
		{
			width /= 2;
			height /= 2;
		}

		Screen.SetResolution(960, 540, !m_IsStandAlone);
    }

	private void GetResolution(ref int i_Width, ref int i_Height)
	{
		switch(m_ScreenResolution)
		{
		case ScreenResolution.Landscape:
			i_Width = 1920;
			i_Height = 1080;
			break;
		case ScreenResolution.Portrait:
			i_Width = 1080;
			i_Height = 1920;
			break;
		}
	}

    private void ApplicationDidReceiveMemoryWarning()
    {
        Debug.Log("Application received memory warning");
        Resources.UnloadUnusedAssets();
        System.GC.Collect();
    }
}
