using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour, IDependenceOnStaticEvents
{
	[SerializeField] SpriteRenderer spriteRenderer;

	public float Radius
	{
		get
		{
			return radius;
		}
	}

	public int TeamIndex
	{
		get
		{
			return teamIndex;
		}
	}

	public bool IsAlive
	{
		get
		{
			return isAlive;
		}
	}

	private float radius;
	private int teamIndex;
	private bool isAlive;
	private float speed;
	private float speedMultiplier;
	private Vector2 moveDirectionNorm;


	public void Initialize(int teamIndex)
	{
		this.teamIndex = teamIndex;

		ToggleAliveState(true);
		UpdateSpeedMultiplier();
		UpdateColour();
		SetRandomRadius();
		UpdateRadiusAppearance();
		SetRandomSpeed();
		SetRandomMoveDirection();
		SubscribeToEvents();
	}

	public void Initialize(UnitData data)
	{
		this.teamIndex = data.teamIndex;
		radius = data.radius;
		speed = data.speed;
		moveDirectionNorm = data.moveDirection;
		transform.position = new Vector2(data.posX, data.posY);

		ToggleAliveState(true);
		UpdateSpeedMultiplier();
		UpdateColour();
		UpdateRadiusAppearance();
		SubscribeToEvents();
	}

	public void SetSpeedMultiplier(float speedMultiplier)
	{
		this.speedMultiplier = speedMultiplier;
	}

	private void UpdateRadiusAppearance()
	{
		transform.localScale = new Vector3(radius * 2f, radius * 2f, 0f);
	}

	private void UpdateSpeedMultiplier()
	{
		speedMultiplier = Battle.battleSpeedMultiplier;
	}

	private void UpdateColour()
	{
		string colourKey = GameConfig.TeamsConfig.teams[teamIndex].colour;
		Color color;
		ColorUtility.TryParseHtmlString(colourKey, out color);
		spriteRenderer.color = color;
	}

	public void MoveForvard()
	{
		float previousPosX = transform.position.x;
		float previousPosY = transform.position.y;
		transform.Translate(moveDirectionNorm * speed * speedMultiplier * Time.deltaTime);
		StaticEvents.unitMoveEvent?.Invoke(this, previousPosX, previousPosY, transform.position.x, transform.position.y);
	}

	public void ReflectVerticalMoveDirection()
	{
		moveDirectionNorm = new Vector2(moveDirectionNorm.x, moveDirectionNorm.y*-1);
	}

	public void ReflectHorizontalMoveDirection()
	{
		moveDirectionNorm = new Vector2(moveDirectionNorm.x*-1, moveDirectionNorm.y);
	}

	public void ChangeMoveDirection(Vector2 newMoveDirection)
	{
		moveDirectionNorm = newMoveDirection.normalized;
	}

	private void SetRandomMoveDirection()
	{
		moveDirectionNorm = Random.insideUnitCircle.normalized;
	}

	private void SetRandomRadius()
	{
		radius = Random.Range(GameConfig.BattleConfig.unitSpawnMinRadius, GameConfig.BattleConfig.unitSpawnMaxRadius);
	}

	private void SetRandomSpeed()
	{
		speed = Random.Range(GameConfig.BattleConfig.unitSpawnMinSpeed, GameConfig.BattleConfig.unitSpawnMaxSpeed);
	}



	public void ChangeRadius(float radiusChange)
	{
		radius += radiusChange;
		UpdateRadiusAppearance();
		StaticEvents.changeUnitRadiusEvent?.Invoke(this, radiusChange);

		if (radius <= GameConfig.BattleConfig.unitDestroyRadius)
		{
			Die();
			return;
		}
	}

	private void Die()
	{
		ToggleAliveState(false);
		StaticEvents.changeUnitRadiusEvent?.Invoke(this, -radius);
		StaticEvents.unitDieEvent?.Invoke(this);
		UnsubscribeFromEvents();
		gameObject.SetActive(false);
	}

	private void ToggleAliveState(bool isAlive)
	{
		this.isAlive = isAlive;
	}

	public void SubscribeToEvents()
	{
		StaticEvents.changeBattleSpeedEvent += UpdateSpeedMultiplier;
	}

	public void UnsubscribeFromEvents()
	{
		StaticEvents.changeBattleSpeedEvent -= UpdateSpeedMultiplier;
	}

	public UnitData GetDataPack()
	{
		UnitData dataPack = new UnitData();

		dataPack.teamIndex = teamIndex;
		dataPack.posX = transform.position.x;
		dataPack.posY = transform.position.y;
		dataPack.speed = speed;
		dataPack.radius = radius;
		dataPack.moveDirection = moveDirectionNorm;

		return dataPack;
	}
}
