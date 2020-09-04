using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class GameConfig : MonoBehaviour
{
	public const float BATTLE_SPEED_MULTIPLIER_ON_START = 1f;
	private IConfigStringGetter configStringGetter;
	static private ConfigsContainer battleConfig;

	static public BattleConfig BattleConfig
	{
		get
		{
			return battleConfig.GameConfig;
		}
	}

	static public TeamsConfig TeamsConfig
	{
		get
		{
			return battleConfig.TeamsConfig;
		}
	}

	static public int TeamsAmount
	{
		get
		{
			return battleConfig.TeamsConfig.teams.Count;
		}
	}

	
	private void Start()
	{
		Initialize();
	}

	private void Initialize()
	{
		if (configStringGetter == null)
		{
			configStringGetter = ConfigStringGetterFactory.GetConfigStringGetter();
		}

		Action<string> getConfigStringEvent = DeserializeConfig;
		configStringGetter.GetConfigString(getConfigStringEvent);
	}
	
	static private void DeserializeConfig(string configString)
	{
		battleConfig = JsonUtility.FromJson<ConfigsContainer>(configString);
		StaticEvents.deserializeGameConfigEvent?.Invoke();
	}
}


[System.Serializable]
public class ConfigsContainer
{
	public BattleConfig GameConfig;
	public TeamsConfig TeamsConfig;
}


[System.Serializable]
public class BattleConfig
{
	public float gameAreaWidth;
	public float gameAreaHeight;
	public int numUnitsToSpawn;
	public float unitSpawnDelay;
	public float unitSpawnMinRadius;
	public float unitSpawnMaxRadius;
	public float unitSpawnMinSpeed;
	public float unitSpawnMaxSpeed;
	public float unitDestroyRadius;
}


[System.Serializable]
public class TeamsConfig
{
	public List<Team> teams;
}


[System.Serializable]
public class Team
{
	public string name;
	public string colour;
}


static public class ConfigStringGetterFactory
{
	static public IConfigStringGetter GetConfigStringGetter()
	{
		switch (Application.platform)
		{
			case RuntimePlatform.WindowsEditor:
			case RuntimePlatform.WindowsPlayer:
				return new ConfigStringGetterStreamingAssetsDirectly();
			case RuntimePlatform.Android:
				return new ConfigStringGetterStreamingAssetsWithWebRequest();
		}

		return null;
	}
}


public interface IConfigStringGetter
{
	void GetConfigString(Action<string> callback);
}


public class ConfigStringGetterStreamingAssetsDirectly : IConfigStringGetter
{
	public void GetConfigString(Action<string> callback)
	{
		string path = Application.streamingAssetsPath + "/GameConfig.json";
		string configString = File.ReadAllText(path);
		callback?.Invoke(configString);
	}
}


public class ConfigStringGetterStreamingAssetsWithWebRequest : IConfigStringGetter
{
	public void GetConfigString(Action<string> callback)
	{
		CoroutinePlayer.instance.StartCoroutine(GetConfigStringFromStreamAssetsByWebRequest(callback));
	}

	static private IEnumerator GetConfigStringFromStreamAssetsByWebRequest(Action<string> callback)
	{
		string path = Application.streamingAssetsPath + "/GameConfig.json";
		UnityEngine.Networking.UnityWebRequest request = UnityEngine.Networking.UnityWebRequest.Get(path);
		yield return request.SendWebRequest();
		string configString = request.downloadHandler.text;
		callback?.Invoke(configString);
	}
}




