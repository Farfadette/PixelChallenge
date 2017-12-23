using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class KaboomBackup
{
	private const int NB_PATHS = 1;
	private const int MAX_RETRY = 4;

	private int nbRetry = 0;
	private string[] m_ArcadePaths = new string[NB_PATHS];
	public Dictionary<string, object> mo_JSONData;

	//================================================================================================//

	public KaboomBackup()
	{
		nbRetry = 0;
		if (Application.platform == RuntimePlatform.LinuxPlayer)
			m_ArcadePaths[0] = "/media/root/D/";
		else
			m_ArcadePaths[0] = "D:\\";

//		m_ArcadePaths[1] = "..\\";
		mo_JSONData = new Dictionary<string, object>();
	}

	//================================================================================================//

	public bool Load(string filename)
	{
		if (nbRetry < MAX_RETRY)
		{
			for (int i = 0; i < NB_PATHS; i++)
			{
				if (LoadFile(m_ArcadePaths[i] + filename))
					return true;
			}
		}
		return false;
	}

	//================================================================================================//

	public bool Save(string filename)
	{
		bool bRet = false;

		if (nbRetry < MAX_RETRY)
		{
			for (int i = 0; i < NB_PATHS; i++)
			{
				if (SaveFile(m_ArcadePaths[i] + filename))
					bRet = true;
			}

			if (bRet == false)
				nbRetry++;
		}

		return bRet;
	}

	//================================================================================================//

	private bool LoadFile(string filename)
	{
		bool bRet = false;

		if(File.Exists(filename + ".json"))
		{
			try
			{
				TextReader reader = File.OpenText(filename + ".json");
				string oJSONString = reader.ReadLine();
				reader.Close();
				if (string.IsNullOrEmpty(oJSONString))
				{
					File.Delete(filename + ".json");
				}
				else
				{
					//mo_JSONData = (Dictionary<string, object>)SBK.MiniJSON.Json.Deserialize(oJSONString);
					bRet = true;
				}
			}
			catch (Exception) { }
		}
		return bRet;
	}

	//================================================================================================//

	private bool SaveFile(string filename)
	{
		bool bRet = false;

		/*
		AddHugeString();
		string oJSONString = SBK.MiniJSON.Json.Serialize(mo_JSONData);

		if (File.Exists(filename + ".new"))
			File.Delete(filename + ".new");

		try
		{
			TextWriter writer = File.CreateText(filename + ".new");
			writer.WriteLine(oJSONString);
			writer.Close();
		}
		catch (Exception)	{}

		if (File.Exists(filename + ".json"))
			File.Delete(filename + ".json");

		if (File.Exists(filename + ".new"))
		{
			File.Move(filename + ".new", filename + ".json");
			bRet = true;
		}
		*/

		return bRet;
	}

	//================================================================================================//

	private void AddHugeString()
	{
		mo_JSONData.Add("HugeString", "xF35KLCRFDf4UrQhuARZ fS7LEQIw4q7yscfRQYsk kbC451LwbMnI5FyCWSWx b2r9seOSVOJrg5nInKqb 0GzKQgl3qImtpaXHUA2m tTygk3zcw2RUpf3Cci1W vHx2xvGKztzfXAnOcpcW 2M8TFh5nAlTrntDs9arL xxCBAFKfwlU0nls26aLn EGFRAqBvt1WXSTiV2wJZ VAAiuu7kml1ooblyvUTw tfqNkuOAevO8qObjzQkS 2hJgX62CplPys5YzxwzG z0H9n44GSw9QzEJYX18z 5D73DZmytuyWnLJAcaRZ KyE29GKdbMULDaVDk12b cE0DWkAGbcRewIKUZW6D wxagtffbUy202dtQB9et gdJrnHKjScNMGL2lkYzQ bUXPvYsuhQqqAcxG3Yqe 8pcwS90qAU3lpvKFRc8D pJSiIrT9pEO4CNpgOtYZ KGaILl4tWb38XJE5JUdh l9FMdL4N1LPdZsfqMfg6 XnPyRgdavNdcvGPBWwtY P4n1o6unxy7az3wBdUhl oLOP2nwniBXSUanfkuFP jMVuoJNTmoTsaH69WmNu bdlC3y7gIPhbTd2ppmdc YkAsi1XaFudYD8Se5GCb xIVrnGA4W7gjXdWPcC2j OM05dEbjegLkWeJEa9et rmqIxmAa1FF2yixkWk0N XK4qDb8n4EpV3eXvNbog ObCaHZUnuQtlC95NYYz9 yoCJH60aJr3q2sn7aQVr K2YwWXYUDG0Fxaxx6r4V of04U68Cz9LJqGduOnUE 31LgK2Ysrx8EXbWOgC4P Xz5eFx1mkfOmkmgIeetX bmC4rHb6zNew5zVyHahe CTqvYD674pYNzIqMSevz NPnpoKsYYw2QCU5QrG2b Df54ak2Ej7KxDwXjskHZ CvCHCAPz7zZ75EH1vyC0 9iWkctbliGO59MHp6KPZ z8iugaJcJFAFcErnXFO5 U77EkRuryO03YVMmxVfG O2tvds87hBnxrfyUR2kj oQPihImANeTWYzs7kExG qSgDfR5Oe782kXkr8xLa MYLX6bMFAH8GSHrxovd5 Q58alYJPMsQw9KOyvlCg 5eH6O8D9QGu7SIIoHtSH 7BY3lK9kLfPw7I4Gc492 WgznRcn6rrdg2gyk92So QFq4YsYVVXPZORsMi6bj hQqKtP2wOk1jD7tnkLcf I8ZQxdJzn849z4z2gZDe dEnjrHBHXwczKmhJHxmZ T98bb8JhbmrHr8FDLimA NRdWJDogYFIQk523yIBN KxiaVoz1mww9cyqQBnER 0AcYcXNVvdpwC54Cxg2Q ldqwG3sTUUUEHxtiVBlN BpfbxKEp2JMsrGV8W4we eOeHUbUcYrQ6ZICQeiCX fGQxXynvd7o6JoWCQ7aF I5uXlamxWIoFZQpkhrdK qdZVS9VrHl3mpa1r3BYb DYuj7EaMoGJSS2iYQhMz 8Cth0yQojiDYTbI3OEJ9 1IjhqrHHQSPPjSZmGECj 0Z3PYUuUDYi5WiYnXCBC jULkpmeHY8wFDPwV3R0Y e8vrIeTdWbFpVtoNYqdx 0QyHZu6A12as42xhaZcW pKmyzvVLkbdUCnK0dD4B 7wn5xcR2qJgHI8oRXo7D iMPMmHXaHGatqcBHH6sZ 8bcD4Qak3DJBylj752xY Zc04aDKOaHdOZ1nShnYT g0XxmV9EVkgHiK1y3Wdw gAcUk12Zn93SU8RyuHxP 4PpZXXF1z0Noxr7znwEv VI1QZDP8OoO4rtfuGkno zuUNPcP1bZeUy6QSyGrn YdByFzSH5YJEG6dpvvW0 uoqgG6eDoQz7XFnTl9ZJ AWsbnm93EpDii1CiULBa RJBW7R7y1LxLhAfBfKxb Lyh53QQvPu5BITfjq4er nIGe6BfcZwPV8tq8TqDv PpYjGLagZNjZtwRNje7j vtTOMYWnTJqeOezrEsqx gNkjWBxt0CGZjurlSh9a jcvRgiy8bw0wH06XZK1c MSvOS77YX3XDsGp9D6uq pYQsfFDxy6yjTFBsMWTo LT6BR0Xb7ffFJoGZVgLg C4Fu76aa26JiZaJfKDTn wIjlvnkpRU7zfN2FGHx7 i6GquwiMNgg38W4Gg8a2 wd6NcBloaI7u8PsIgzKO uoOIrjHpr1kAOvx2tRZw ZzJyqA15zjX7FqbfeqzR vYlya1dMfx5B0LaEiYVq BLyvE1akQ2BT94k8g8ky yHG9p0LIDmMtnN0nCoRn Kz82vnwsJIU6hz79RrHc j49cqmoQUzIRbjtja2p2 htpcDLNMUro0Z0uRJmPi rUCY33rqY2q6udruBAJi arpgbBJEk6oBKc5wQGRc 279buLNx9wCieU36TN8C Ij2aP4wl6KYSoQfb180A 2Pa4KiamdFrddW3dn3XB Id4fONebdSDLdKM8kKAr C34xsXmiamyQsNpLft3F 63554YPNL5bHKXXISBpN lEzxANYmj5cFDMmaKMVd QTugSVSWaeh71UfmPn3S uDinFXc2Hy2UQBtORsvF jIEsrCti0DZihnsRLc1y mNda1SP0Dxt0QleEyMF4 6jguwDcWgsUtuU0zdBFj aSsmm5m3MtgeQ1bxW4XO teeEVWkjGDp6BW3cbNEY ku1XUYDYRrpRDoElSlhp KfBXv8AI8sKik2hG1OsY rUNy27fy13kxFB8Dbmvd oIsmuRr4zFP0XkZH2jgS MmUGYReCpVsX2NXumyMc 6qI45tDZe8sWpvIlgzQf hzaWSGwDJZC7kKjYMKFt 14etKgtbl7p4hpR50Xrz ctg5ryBDh248u6xZk3oo lOvrQriH8Eo1UDsDtdfL bVBHPwCfqFIoNWvLvxHk dyDGSdEn6fMXy8MVqnhD HUeAvsBnlfFZjWoxDH38 d40UjL7Oxodipf4BNUHZ GMY8J9nIynYAOyxTqyOg WGiqvH0hAnPByvhcIYqK ro3noytt7Z3wZwi7ie8p DHGbdOSRdrJyyDl5LjF2 uNA8wSK1jAPwheq7FLtj CccAVzfCO3tZ7djUnshP Muolg6haFlEg8DzThMIf s0ttOCrO6esWvXw76k4u UsVkaAyPmZigGBCQq45j LWO41xbKJGGSQ4Y2LLYk pODv2i5DxiRtHVdLN4Cm 7fXHYaujBSEYLEWsfA2w 4oDeadLSZtPNjJtj6nhi xxtGmkxG5HtPGRmdbXD1 mszh88JCJoJ8QGKxH6Bh ZRuvilPJ70rGUFJx9DUG j4lb7W1SVZkHqOfn9dwo Vh9531QeaNwKVmfDkpmu btlcw1BImKhXKXITLNgF Dm56PdKF3FhajAUrmMw2 887mTImY207eKrTVSFAU UO2JlMYfAZ18i6alVKaQ ill5OHYfhUGtSw5Cao2h hFuEwU6jY7jHOcYzjMMg lr2TraLgzen0lx5nMgWE 1dWM2aHwxfcqIdD1IKiR E7jJdHceYQRjnbrTbnU0 rFABet7BfHhIDF6ItHOj KDgVVXNSVCOU7PhwwOpn IlDaxK3Sybx3NJp5C62x zmMPxWAOFnLcxADbPxOl tjzpykKFX5ZwJhIIlMMJ z7jJICvViMUI5QynGawR J2zcd7ietf4Xc4XWAFi6 WNFykF4hGRFzpEPBN6ku akbVxdwKyKMnPSlpjoDl iX6oE2jTpHZ7QxxE1c0d dljIkeFTNBuyVEhOXXxo VBkHYZfnnKAD9WNavuB2 x8fR9zMVpYOxlolGhaIS vHXtJHuAw1AJYTYpeeg8 psmbeDyRB13DDX9X9xnS MrMnRBH8KAc2AIKgNoQt 5GfV2MjktJs5YugCvYTL LQRZ19nEXNedVH05JRB7 VJDtIfNBnW8jBD6hawQM f6iUKoWJY85ZW5dIiBoW gJm0DA8neoajnIPw48aa PTKnprR1lqQbAqKKarXa Sqguh0thFWcWnDTfF0MV RLuysI7xrgVfcrTbtCXq 0WBdHNyAznok3UOBOIhY vDKuuVZpcsdHhcdjaXTv bj4vDqBmYRshM48R0nug 9G8oqEqwyq4Zs6kxOUE2 bEfL405MbrXQLx35biKE E1HXSlXHbxXSRnoWvqQp ef8c4rjrZ95lHRXtRsIv pUizxVlpa7k68aSlclpx m7PDZF9HlN9bEOrsL6lO knwx9QqbgmIIrgNE65Ns tlQU61u2BuRnarmTSFaL vzDljjwXA8Ej3eMsktGC l3dTlfkKzBZkIiEOwivA knfsvJ8udOIiarB96CSx uXasbxfQ2VKyLnaae4gq IA0s9SYsbrx3piDETObA FPUCdjxvi6C6wencfsS7 ojbjBWS1XlPz2aSKCGxt Agbg9ICt1LRIxzZs453V FyZdM48tDe9cOpGisXlU 6eHcbjLEzDNrziBG29jY Grxcn6JRqtlIABs8HsqJ B7azJQAx0Dfc8mf4fCMK q32rXsOpxGPkBNeIByyR iWe68a7mlFtEwDyFc6ue JZWlGm8T83z4IQ82HXVF SJhaw8fmsvAw0CPIa465 nA7aUdaXioSZEaltW0Cv ZzLg7EcLqeMUGY33bMZ9 dYE76muwlLD8tb2fQwhm hxIYyRKeWtfJ9kUGWK9P p3D5bviuKnvEjJh0phKX 5YIRxhNhgttoaF27VliN rgPDKXpbwwXwGgucBy0b tKVol1id56UWk398EAsf AW7NjLRrD67AZasSepxR oiIAqFidDKkl7QpGo8kb eqYgixlHCMGMEBNBBP5H jzYNaTvi1nFtbTijHBCX eOIjfkkXItbJgY7qawtO gd7USI5T7ngQovHd6PmX 9GihHmfoZlqGR3Rf3pTc pVuWVNTNkCx3jKCCUcZ2 0RPDdG1h50BYvT2DM6kO tuskvrjbsenhonGFUa9g cakcKYmCg27Vc6HOlg7z 89J6w12XL7L8u69i9YPG p202Y6ZpBpMcW85wQt3s nSNHPtacdertRcLrRFau ym5t9sbx6HzmaJjUpURj xCZe5oeYjIVftv1inIEb 5oggl5hKFdBZxCpiJsiE YSHfapFwdz8zq22K4XwA hs2SoQco0HnsWbEjCCKt 4nM9FWVjtV7g8MNf514r EmxABU3c2R6ZS7aVwVwF nKO1r0NOQF0I9JXYPXOv 896XH3NmmxTBqetYLUDp Tl6nbW2tDscBIfxX6jwn bVTLK021GXFfjKxwtxCU RFtwqGJ35kLVzakQ5dgS BiAy737lfQucogBNr0nv uKAKYvmBV2iuyE0s8YYX CprnVjAwG7QBuPoIE9bs 9GvWkWQPbdQ17S3OYuCN E93cDyOLCTmWzn6RWQNs tWCXdy6K1iCXINtN3TNO k2apoT6rVGBCLMFalso4 Tp9JYe6vSJZEShZ61Q5C hmAUywVls1KbtYUyEexj wdysYgDK71VKarSe7tQh p89JsHNRSGfkKuTki8TU WROJ7qlG11LVmhxcgnEI U8QBru5hJEgY40to2W1A UxrkT1EY1ogKtZZkxCQC ksHgc3ovIiVl5124t5jc ucgNu8hCTZb8M6jI6eXJ 4ixufsS53cTN8tz2Iare 0C9CcNVv7iJlEsgEbZcy 7sQz80M9czQn5uuCad6n ZQEmdKEkTm5PEoTGagNz FycOpWc9RRKMcsvJ8vKx 0vtyswfIAtqgynJBgMP2 fh2ng5seooCLZPC3yuNl oYvARCam2T0rCAvfLy3X jhW8ColIji6v32wyQM61 Sn1Fl9N9VMuqT2IFe5rM hhxCeAc4HSp5QboqoulS pP736yKqjGL6fLFeWjJb O01Zk2TOzalcpxe249Xm ckeBxjYLjLWeVKjJdG13 7DzIE74DJWiZXTh2Urkq TgxjSplwk8UwFhA8Y3sC jrR7yUMaf143pHzWmpJd AG08swYYuNI4EP2nTz0i SkklSh8j0NhpWVN87Tyj f9xP0eXXv5hFXwSt1tJu a5MsapLTrYeFkiKcZCwJ i7dz9nmI5CMfXjpVvDGc ifMl6Qr1kc2eA30UE88o qXxC4f02n16J7X0CqYe6 tnEwDDl1PCiai49TryOg C6dPZY3ZGAcfYxZOs4ep n74aksyaL3a8Bn21SY8F 2q7PRggTw0OLVl8cgOdd vSnVPOhyCSx4biAmp9oK YSdYrMU3d6lBTzK4G7YT P5HMqxYORm6i3KDbjnx0 qtH6DQsmyNr4aWjumBGY ovxGl6aU32NQ4EMGryXs 3key42cTAZD5WmeeMHq8 Hwb6iexlyqIIUin1XXiI xDWWqLuChyLrNyNP1fJV FmbmqeKI6DsJ8IaZVjbC aVjRLacRM3s80z1pX5Qd aPCsW4CGD77C6oZcpvSL ymOwjmET5QF3lALpB9cU mpzmXqMKG58CgEwJpskx WqmlV8kXPpKtm9uN5o9B vgvRbluaLgvSn8gNbaYI oKQSjD8OuR54cIn0HeaN dclaISwuXbAIJNoHRXhn 54rHbyymtcwmdTsyDL4f Z9L5b7QwAUMMbbGxVOdn Y02tOQuKSTcyVCB6x7Ks 7raZQlAukIRcHnUkCD0Z nXr9gr2Euqlv5z4RjWCW reRgAlqiPekSBlMAXVm3 yGgA3Qv8Ej3ZD7B0NegV Yery0kZuLhAlrDF5GVOT KwwhjmF6q0XMQKig5Yyx UztWe9fDmr35iHsNbPPH qELsswdvIXGWRrJ3VwcF IChMusOVsJlX59sdgyOH jclfx0B8i5ThfJnFXRsS 4O5BeEfHICVZKJwfWpGF LeGRqsdEqC80277WbH6p hzCcpmYYf2cCjb4HDdQN gOxJAH8TiLA85MqF4vmu KilPnJrs3Lzrrzi0ysfY nS7X9rysdGVZangrJ42C 6Ocyh2Ek68xvHsxkmo8K 4mdnA1sZrd9ZrpWehRUB uMGxoxJXQeXzsi71HSxR hkHQUHKoOgqYAUAfrCdy tGP3z5i1AKgddB96ujqo Tov9Ao36jQRgNGbVS9RI D5AWEhajxUwyprf7xIZ6 xDLJnY13uM8jVRx8TYnk VmuVnBzrA9prR2zmElyO xvjy8j17OAYMKVcL0YdK Fs4dz4I584PUC8RCFRwC ouOCL9f2X2wvYiaY945B yZlXhsorYo1HZLCn4RVq o0SfEJjRtqT9vRzilESg KPyNe2fxCkvazDFuaal6 7NljGZHL5BZkHHgu6SnT jsJ2uS2Nbb0JBgEcnEDU lbK7cnEn2C7kqwQ7WOL6 bGq0FAnxzfI5tB9XHAyO oJihmUu71XEKp6bk41vK lki18ZaSdIWs25doJKxE kWhlZDANE1In7EWvGFAL cZWhWX0c4MXm5XVc9o6i VEltVSnvqsqfLhY6o0SC MQyyGDfuMRG0TFIRDKKX DmyX0Tcm0bgLABylOd5d 5tPayeJpAMoQurwHqWHQ SdyfHEsCSTtqc2bxpnpU isKZ3ZATDDgN7Gy1PX9r ctl1UZjGD0LC7AkgVm3R fZZ2uBj9B30QakzfZuJI Ez2KVQGvXMZ0HSI4cklz uTDd6OwPv1Ttpdtx9qVg xXboi5GNpWzVyjBWNGnH IsB78YbHXl8TkO2p0HBz PZrNqEpAyYcY0wjilvKD FKG2zNfc2QZpyBHXgjFv Jsa17BSk9uAha0GVs3AO Nxc9nqTAKYZesNOzufZq zEPnCgzZV0P7x8wmQfPS TH5vvpxD8dUOFuiZmD58");
	}
}