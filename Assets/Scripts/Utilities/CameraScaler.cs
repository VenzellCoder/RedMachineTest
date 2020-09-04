using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScaler : MonoBehaviour, IDependenceOnStaticEvents
{
	[SerializeField] private Camera camera;

	private DeviceOrientation currentDeviceOrientation;
	private float currentScreenHeight;
	private float currentScreenWidth;


	private void Awake()
	{
		SubscribeToEvents();
	}

	private void Update()
	{
		ProcessDeviceOrientationChange();
		ProcessScreenSizeChange();
	}

	private void ProcessDeviceOrientationChange()
	{
		if (DeviceOrientationWasChanged())
		{
			currentDeviceOrientation = Input.deviceOrientation;
			UpdateCameraScale();
		}
	}

	private void ProcessScreenSizeChange()
	{
		if (ScreenSizeWasChanged())
		{
			currentScreenHeight = Screen.height;
			currentScreenWidth = Screen.width;
			UpdateCameraScale();
		}
	}

	private bool DeviceOrientationWasChanged()
	{
		return Input.deviceOrientation != currentDeviceOrientation;
	}

	private bool ScreenSizeWasChanged()
	{
		return (currentScreenHeight != Screen.height || currentScreenWidth != Screen.width);
	}

	private void UpdateCameraScale()
	{
		DoHorizontalAlignment();

		if (!IsBattleAreaTopSideInCameraView() || !IsBattleAreaRightSideInCameraView())
		{
			DoVerticalAlignment();
		}
	}

	private bool IsBattleAreaTopSideInCameraView()
	{
		Vector3 viewPos = camera.WorldToViewportPoint(new Vector2(0f, GameConfig.BattleConfig.gameAreaHeight / 2f));
		return viewPos.y <= 1;
	}

	private bool IsBattleAreaRightSideInCameraView()
	{
		Vector3 viewPos = camera.WorldToViewportPoint(new Vector2(GameConfig.BattleConfig.gameAreaWidth / 2f, 0f));
		return viewPos.x <= 1;
	}

	private void DoHorizontalAlignment()
	{
		camera.orthographicSize = GameConfig.BattleConfig.gameAreaHeight / 2f;
	}

	private void DoVerticalAlignment()
	{
		camera.orthographicSize = GameConfig.BattleConfig.gameAreaWidth * ((float)Screen.height / (float)Screen.width) * 0.5f;
	}


	public void SubscribeToEvents()
	{
		StaticEvents.deserializeGameConfigEvent += UpdateCameraScale;
	}

	public void UnsubscribeFromEvents()
	{
		StaticEvents.deserializeGameConfigEvent -= UpdateCameraScale;
	}
}
