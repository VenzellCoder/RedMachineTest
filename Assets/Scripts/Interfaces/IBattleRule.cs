using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IBattleRule
{
	bool IsBattleEndConditionMet();
	void SetUnitsForRuleChecking(Units units);
	int GetWinnerTeamIndex();
}
