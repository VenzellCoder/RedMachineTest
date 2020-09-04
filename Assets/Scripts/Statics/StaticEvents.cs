using System;

public static class StaticEvents
{
	public static Action deserializeGameConfigEvent;

	public static Action createNewBattleEvent;
	public static Action<BattleData> createBattleFromSaveEvent;

	public static Action saveBattleEvent;
	public static Action loadBattleEvent;

	public static Action<Units> unitsArePreparedForBattleEvent;
	public static Action startBattleEvent;
	public static Action finishBattleEvent;
	public static Action<BattleResult> hasBattleResultsEvent;

	public static Action<Unit, float, float, float, float> unitMoveEvent;
	public static Action changeBattleSpeedEvent;
	public static Action<Unit, float> changeUnitRadiusEvent;
	public static Action<float[]> updateUnitsRadiusSumsEvent;
	public static Action<Unit> unitDieEvent;
}
