using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStatistics : IDependenceOnStaticEvents
{
	private float timeStamp;
	private float[] teamRadiusSums;


	public BattleStatistics()
	{
		SubscribeToEvents();
	}

	public void StartCollectStatistics(Units units)
	{
		StartTimer();
		UpdateRadiusSumsOnStart(units);
	}

	public int GetTimeInSeconds()
	{
		return (int)(Time.time - timeStamp);
	}

	private void StartTimer()
	{
		timeStamp = Time.time;
	}

	private void UpdateRadiusSumsOnStart(Units units)
	{
		if (teamRadiusSums == null)
		{
			teamRadiusSums = new float[GameConfig.TeamsAmount];
		}

		for (int teamIndex = 0; teamIndex < GameConfig.TeamsAmount; teamIndex++)
		{
			teamRadiusSums[teamIndex] = 0f;
		}

		for (int i = 0; i < units.Count; i++)
		{
			teamRadiusSums[units[i].TeamIndex] += units[i].Radius;
		}

		StaticEvents.updateUnitsRadiusSumsEvent?.Invoke(teamRadiusSums);
	}

	private void OnChangeUnitRadius(Unit unit, float radiusChange)
	{
		teamRadiusSums[unit.TeamIndex] += radiusChange;
		StaticEvents.updateUnitsRadiusSumsEvent?.Invoke(teamRadiusSums);
	}

	public void SubscribeToEvents()
	{
		StaticEvents.changeUnitRadiusEvent += OnChangeUnitRadius;
	}

	public void UnsubscribeFromEvents()
	{
		StaticEvents.changeUnitRadiusEvent -= OnChangeUnitRadius;
	}
}
