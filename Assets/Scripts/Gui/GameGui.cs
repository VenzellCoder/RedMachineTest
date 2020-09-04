using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGui : MonoBehaviour
{
	[SerializeField] private BattleControlPanel battleControlPanel;
	[SerializeField] private BattleResultPanel battleResultPanel;
	[SerializeField] private BattleStatusPanel battleStatusPanel;

	private void Awake()
    {
		battleControlPanel.SubscribeToEvents();
		battleResultPanel.SubscribeToEvents();
		battleStatusPanel.SubscribeToEvents(); 
	}

}
