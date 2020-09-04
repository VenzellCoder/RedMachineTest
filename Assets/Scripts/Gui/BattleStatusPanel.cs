using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleStatusPanel : MonoBehaviour, IDependenceOnStaticEvents
{
	[SerializeField] private List<Image> sliders;


	private void InitializeSliders()
	{
		if (GameConfig.TeamsAmount != sliders.Count)
		{
			Debug.LogWarning("Teams amount doesn't match with sliders amount. \n GUI top panel won't work!");
			return;
		}

		for (int i = 0; i < sliders.Count; i++)
		{
			string colourKey = GameConfig.TeamsConfig.teams[i].colour;
			Color color;
			ColorUtility.TryParseHtmlString(colourKey, out color);
			sliders[i].color = color;
		}
	}

	private void UpdateSlidersAppearance(float[] radiusSums)
	{
		if (radiusSums.Length != sliders.Count)
		{
			return;
		}

		float allRadiusesSum = 0;
		foreach(float radiusSum in radiusSums)
		{
			allRadiusesSum += radiusSum;
		}

		for(int i = 0; i < sliders.Count; i++)
		{
			sliders[i].fillAmount = radiusSums[i] / allRadiusesSum;
		}
	}

	public void SubscribeToEvents()
	{
		StaticEvents.updateUnitsRadiusSumsEvent += UpdateSlidersAppearance;
		StaticEvents.deserializeGameConfigEvent += InitializeSliders;
	}

	public void UnsubscribeFromEvents()
	{
		StaticEvents.updateUnitsRadiusSumsEvent -= UpdateSlidersAppearance;
		StaticEvents.deserializeGameConfigEvent -= InitializeSliders;
	}

}
