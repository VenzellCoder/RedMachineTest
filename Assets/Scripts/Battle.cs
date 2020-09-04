using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle : MonoBehaviour, IDependenceOnStaticEvents
{
	private Units units;
	private BattleSimulator simulator;
	private IBattleRule rule;
	private BattleStatistics statistics;
	private BattleSaveLoad saveLoad;

	static public float battleSpeedMultiplier = GameConfig.BATTLE_SPEED_MULTIPLIER_ON_START;


	private void Awake()
	{
		CreateNonMonobehaviours();
		SubscribeToEvents();
	}

	private void Update()
	{
		simulator.TrySimulate();
	}

	private void CreateNonMonobehaviours()
	{
		units = new Units();
		simulator = new BattleSimulator();
		rule = new BattleRuleLastTeam();
		statistics = new BattleStatistics();
		saveLoad = new BattleSaveLoad();
	}

	private void OnCreateNewBattle()
	{
		simulator.StopSimulation();
		units.PrepareUnitsForBattle();
	}

	private void OnCreateBattleFromSave(BattleData battleData)
	{
		simulator.StopSimulation();
		units.PrepareUnitsForBattle(battleData.unitsData);
	}

	private void OnUnitsArePreparedForBattle(Units units)
	{
		simulator.SetUnitsForSimulation(units);
		simulator.StartSimulation();
		statistics.StartCollectStatistics(units);
		rule.SetUnitsForRuleChecking(units);
		StaticEvents.startBattleEvent();
	}

	private void OnBattleFinish()
	{
		simulator.StopSimulation();
		CreateBattleResult();
	}

	private void CreateBattleResult()
	{
		int winnerIndex = rule.GetWinnerTeamIndex();
		int battleDuration = statistics.GetTimeInSeconds();
		BattleResult battleResult = new BattleResult(winnerIndex, battleDuration);

		StaticEvents.hasBattleResultsEvent?.Invoke(battleResult);
	}

	private void OnSave()
	{
		saveLoad.SaveBatle(units);
	}

	private void OnLoad()
	{
		saveLoad.TryLoadBattle();
	}

	public void SubscribeToEvents()
	{
		StaticEvents.createNewBattleEvent += OnCreateNewBattle;
		StaticEvents.createBattleFromSaveEvent += OnCreateBattleFromSave;
		StaticEvents.unitsArePreparedForBattleEvent += OnUnitsArePreparedForBattle;
		StaticEvents.saveBattleEvent += OnSave;
		StaticEvents.loadBattleEvent += OnLoad;
		StaticEvents.finishBattleEvent += OnBattleFinish;
	}

	public void UnsubscribeFromEvents()
	{
		StaticEvents.createNewBattleEvent -= OnCreateNewBattle;
		StaticEvents.createBattleFromSaveEvent -= OnCreateBattleFromSave;
		StaticEvents.unitsArePreparedForBattleEvent -= OnUnitsArePreparedForBattle;
		StaticEvents.saveBattleEvent -= OnSave;
		StaticEvents.loadBattleEvent -= OnLoad;
		StaticEvents.finishBattleEvent -= OnBattleFinish;
	}

	[ContextMenu("Clear player prefs")]
	private void DEBUG_ClearSaves()
	{
		saveLoad.Clear();
	}
}


[System.Serializable]
public class BattleResult
{
	public BattleResult(int winnerTeamIndex, int battleDurationInSeconds)
	{
		this.winnerTeamIndex = winnerTeamIndex;
		this.battleDurationInSeconds = battleDurationInSeconds;
	}

	public int winnerTeamIndex;
	public int battleDurationInSeconds;
}