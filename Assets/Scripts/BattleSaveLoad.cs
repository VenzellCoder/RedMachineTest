using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSaveLoad
{
	private const string PLAYER_PREFS_BATTLE_KEY = "LastSavedBattle";


	public void SaveBatle(Units unitsInBattle)
	{
		BattleData battleData = new BattleData();

		for (int i = 0; i < unitsInBattle.Count; i++)
		{
			battleData.unitsData.Add(unitsInBattle[i].GetDataPack());
		}

		string battleDataString = JsonUtility.ToJson(battleData);
		PlayerPrefs.SetString(PLAYER_PREFS_BATTLE_KEY, battleDataString);
	}

	public void TryLoadBattle()
	{
		string battleDataString = PlayerPrefs.GetString(PLAYER_PREFS_BATTLE_KEY);

		if (battleDataString.Length == 0)
		{
			return;
		}

		BattleData battleData = JsonUtility.FromJson<BattleData>(battleDataString);
		StaticEvents.createBattleFromSaveEvent?.Invoke(battleData);
	}

	public void Clear()
	{
		PlayerPrefs.SetString(PLAYER_PREFS_BATTLE_KEY, "");
	}
}

[System.Serializable]
public class BattleData
{
	public List<UnitData> unitsData = new List<UnitData>();
}

[System.Serializable]
public class UnitData
{
	public int teamIndex;
	public float posX;
	public float posY;
	public float speed;
	public float radius;
	public Vector2 moveDirection;
}