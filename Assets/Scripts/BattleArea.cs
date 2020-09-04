using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleArea : MonoBehaviour, IDependenceOnStaticEvents
{
	[SerializeField] private Transform area;


	private void Awake()
	{
		SubscribeToEvents();
	}

	private void UpdateSize()
	{
		area.localScale = new Vector2(
			GameConfig.BattleConfig.gameAreaWidth, 
			GameConfig.BattleConfig.gameAreaHeight);
	}

	public void SubscribeToEvents()
	{
		StaticEvents.deserializeGameConfigEvent += UpdateSize;
	}

	public void UnsubscribeFromEvents()
	{
		StaticEvents.deserializeGameConfigEvent -= UpdateSize;
	}
}
