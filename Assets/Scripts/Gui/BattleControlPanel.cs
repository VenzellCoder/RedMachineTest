using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleControlPanel : MonoBehaviour, IDependenceOnStaticEvents
{
	[SerializeField] private Slider battleSpeedSlider;
	[SerializeField] private GameObject loadButton;
	[SerializeField] private GameObject saveButton;


	private void Awake()
	{
		SetPreGameAppearance();
	}

    public void OnPressNewBattleButton()
	{
		StaticEvents.createNewBattleEvent?.Invoke();
	}

	public void OnPressSaveBattleButton()
	{
		StaticEvents.saveBattleEvent?.Invoke();
	}

	public void OnPressLoadBattleButton()
	{
		StaticEvents.loadBattleEvent?.Invoke();
	}

	public void OnChangeBattleSpeedSlider()
	{
		Battle.battleSpeedMultiplier = battleSpeedSlider.value;
		StaticEvents.changeBattleSpeedEvent?.Invoke();
	}

	private void SetPreGameAppearance()
	{
		battleSpeedSlider.gameObject.SetActive(false);
		loadButton.SetActive(false);
		saveButton.SetActive(false);

	}

	private void SetInGameAppearance()
	{
		battleSpeedSlider.gameObject.SetActive(true);
		loadButton.SetActive(true);
		saveButton.SetActive(true);
	}


	public void SubscribeToEvents()
	{
		StaticEvents.createNewBattleEvent += SetPreGameAppearance;
		StaticEvents.startBattleEvent += SetInGameAppearance;
		StaticEvents.finishBattleEvent += SetPreGameAppearance;
	}

	public void UnsubscribeFromEvents()
	{
		StaticEvents.createNewBattleEvent -= SetPreGameAppearance;
		StaticEvents.startBattleEvent -= SetInGameAppearance;
		StaticEvents.finishBattleEvent -= SetPreGameAppearance;
	}
}
