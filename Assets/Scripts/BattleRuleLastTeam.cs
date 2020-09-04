public class BattleRuleLastTeam : IBattleRule, IDependenceOnStaticEvents
{
	private int[] unitsAmountInTeams;
	private Units units;
	private int winnerTeamIndex;


	public BattleRuleLastTeam()
	{
		SubscribeToEvents();
	}

	public void SetUnitsForRuleChecking(Units units)
	{
		this.units = units;
		UpdateUnitsAmountInTeams();
	}

	public bool IsBattleEndConditionMet()
	{
		return (GetAliveTeamsAmount() <= 1);
	}

	public int GetWinnerTeamIndex()
	{
		return winnerTeamIndex;
	}

	private void UpdateUnitsAmountInTeams()
	{
		unitsAmountInTeams = new int[GameConfig.TeamsAmount];

		for (int i = 0; i < units.Count; i++)
		{
			unitsAmountInTeams[units[i].TeamIndex] ++;
		}
	}

	private void OnUnitDie(Unit deadUnit)
	{
		unitsAmountInTeams[deadUnit.TeamIndex]--;

		if (!IsBattleEndConditionMet())
		{
			return;
		}

		winnerTeamIndex = GetAliveTeamIndex();
		StaticEvents.finishBattleEvent?.Invoke();
	}

	private int GetAliveTeamsAmount()
	{
		int aliveTeamsAmount = 0;

		for(int i = 0; i < unitsAmountInTeams.Length; i++)
		{
			if (unitsAmountInTeams[i] > 0)
			{
				aliveTeamsAmount++;
			}
		}

		return aliveTeamsAmount;
	}

	private int GetAliveTeamIndex()
	{
		for (int i = 0; i < unitsAmountInTeams.Length; i++)
		{
			if (unitsAmountInTeams[i] > 0)
			{
				return i;
			}
		}

		return -1;
	}


	public void SubscribeToEvents()
	{
		StaticEvents.unitDieEvent += OnUnitDie;
	}

	public void UnsubscribeFromEvents()
	{
		StaticEvents.unitDieEvent -= OnUnitDie;
	}
}
