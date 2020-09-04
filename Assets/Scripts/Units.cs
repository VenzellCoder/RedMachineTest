using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Units : IDependenceOnStaticEvents
{
	private GameObject unitPrefab;
	private List<Unit> units = new List<Unit>();

	public Unit this[int i]
	{
		get
		{
			return units[i];
		}
	}

	public int Count
	{
		get
		{
			return units.Count;
		}
	}


	public Units()
	{
		SubscribeToEvents();
		FindPrefabs();
	}

	public void PrepareUnitsForBattle()
	{
		CoroutinePlayer.instance.StopAllCoroutines();
		ClearUnits();
		CoroutinePlayer.instance.StartCoroutine(CreateUnitsWithDelay());
	}

	public void PrepareUnitsForBattle(List<UnitData> unitsData)
	{
		CoroutinePlayer.instance.StopAllCoroutines();
		ClearUnits();
		CreateUnits(unitsData);
	}

	private void FindPrefabs()
	{
		unitPrefab = Resources.Load("Prefabs/Unit") as GameObject;
	}

	private IEnumerator CreateUnitsWithDelay()
	{
		for (int teamIndex = 0; teamIndex < GameConfig.TeamsAmount; teamIndex++)
		{
			for (int i = 0; i < GameConfig.BattleConfig.numUnitsToSpawn; i++)
			{
				CreateUnit(teamIndex);
				yield return new WaitForSeconds(GameConfig.BattleConfig.unitSpawnDelay);
			}
		}

		StaticEvents.unitsArePreparedForBattleEvent?.Invoke(this);
	}

	private void CreateUnits(List<UnitData> unitsData)
	{
		for (int i = 0; i < unitsData.Count; i++)
		{
			CreateUnit(unitsData[i]);
		}

		StaticEvents.unitsArePreparedForBattleEvent?.Invoke(this);
	}

	private void CreateUnit(int teamIndex)
	{
		Unit unit = ObjectPool.GetObject(unitPrefab).GetComponent<Unit>();
		units.Add(unit);

		unit.Initialize(teamIndex);
		SetPositionForNewUnit(unit);
	}

	private void CreateUnit(UnitData unitData)
	{
		Unit unit = ObjectPool.GetObject(unitPrefab).GetComponent<Unit>();
		units.Add(unit);

		unit.Initialize(unitData);
	}

	private void ClearUnits()
	{
		foreach(Unit unit in units)
		{
			unit.gameObject.SetActive(false);
			unit.UnsubscribeFromEvents();
		}

		units.Clear();
	}

	private void SetPositionForNewUnit(Unit newUnit)
	{
		float arenaWidth = GameConfig.BattleConfig.gameAreaWidth;
		float arenaHeight = GameConfig.BattleConfig.gameAreaHeight;

		int attemptCounter = 0;
		int attemptMax = 100;

		do
		{
			attemptCounter++;
			if (attemptCounter >= attemptMax)
			{
				Debug.LogWarning("There is no empty space for this unit");
				return;
			}

			Vector2 pos = new Vector2(
				Random.Range(-arenaWidth/2 + newUnit.Radius, arenaWidth/2 - newUnit.Radius),
				Random.Range(-arenaHeight/2 + newUnit.Radius, arenaHeight/2 - newUnit.Radius));

			newUnit.transform.position = new Vector3(pos.x, pos.y, 0f);
		}
		while (IsUnitCollideWithOthers(newUnit));
	}

	private bool IsUnitCollideWithOthers(Unit unit)
	{
		foreach(Unit anotherUnit in units)
		{
			if (unit == anotherUnit) break;

			if (MathHelper.AreUnitsCollide(unit, anotherUnit))
			{
				return true;
			}
		}
		return false;
	}

	private void OnUnitDie(Unit deadUnit)
	{
		units.Remove(deadUnit);
	}
	
	private void OnGetGameConfig()
	{
		PrepareUnitsObjectPool();
	}

	private void PrepareUnitsObjectPool()
	{
		ObjectPool.CreatePool(unitPrefab, GameConfig.BattleConfig.numUnitsToSpawn * GameConfig.TeamsAmount);
	}

	public void SubscribeToEvents()
	{
		StaticEvents.unitDieEvent += OnUnitDie;
		StaticEvents.deserializeGameConfigEvent += OnGetGameConfig;
	}

	public void UnsubscribeFromEvents()
	{
		StaticEvents.unitDieEvent -= OnUnitDie;
		StaticEvents.deserializeGameConfigEvent -= OnGetGameConfig;
	}
}
