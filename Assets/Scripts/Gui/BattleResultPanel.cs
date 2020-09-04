using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleResultPanel : MonoBehaviour, IDependenceOnStaticEvents
{
	[SerializeField] private Text resultText;


	private void OnBattleFinish(BattleResult result)
	{
		resultText.text = GetResultRichTextString(result);
		gameObject.SetActive(true);
	}

	private string GetResultRichTextString(BattleResult result)
	{
		string winnerName = GameConfig.TeamsConfig.teams[result.winnerTeamIndex].name;
		string winnerColour = GameConfig.TeamsConfig.teams[result.winnerTeamIndex].colour;

		return 
			"Победитель: " +
			"<b><color=" + winnerColour + ">" + 
			winnerName +
			"</color></b>" +
			"\n" +
			"Время боя: " +
			result.battleDurationInSeconds + " секунд";
	}

	public void OnPressPlayAgainButton()
	{
		gameObject.SetActive(false);
		StaticEvents.createNewBattleEvent?.Invoke();
	}

	public void SubscribeToEvents()
	{
		StaticEvents.hasBattleResultsEvent += OnBattleFinish;
	}

	public void UnsubscribeFromEvents()
	{
		StaticEvents.hasBattleResultsEvent -= OnBattleFinish;
	}
}
