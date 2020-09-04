using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSimulator
{
	private bool isSimulating;
	private Units units;
	private SpatialPartitionGrid grid;


	public void StartSimulation()
	{
		isSimulating = true;
	}

	public void StopSimulation()
	{
		isSimulating = false;
	}

	public void SetUnitsForSimulation(Units units)
	{
		this.units = units;

		if (grid == null)
		{
			grid = new SpatialPartitionGrid();
		}
		grid.FullfilGridWithUnits(units);
	}

	public void TrySimulate()
	{
		if (!isSimulating) return;
		SimulateUnitsBehaviour();
	}

	private void SimulateUnitsBehaviour()
	{
		for (int i = 0; i < units.Count; i++)
		{
			ProcessUnitMovement(units[i]);
			ProcessUnitWithBoundariesCollision(units[i]);
			ProcessUnitWithUnitsCollision(units[i]);
		}
	}

	private void ProcessUnitMovement(Unit unit)
	{
		unit.MoveForvard();
	}

	private void ProcessUnitWithBoundariesCollision(Unit unit)
	{
		ProcessUnitWithTopBoundaryCollision(unit);
		ProcessUnitWithBottomBoundaryCollision(unit);
		ProcessUnitWithRightBoundaryCollision(unit);
		ProcessUnitWithLeftBoundaryCollision(unit);
	}

	private void ProcessUnitWithTopBoundaryCollision(Unit unit)
	{
		float boundaryY = 0f + GameConfig.BattleConfig.gameAreaHeight / 2f - unit.Radius;
		if (unit.transform.position.y >= boundaryY)
		{
			unit.ReflectVerticalMoveDirection();
			unit.transform.position = new Vector2(unit.transform.position.x, boundaryY);
		}
	}

	private void ProcessUnitWithBottomBoundaryCollision(Unit unit)
	{
		float boundaryY = 0f - GameConfig.BattleConfig.gameAreaHeight / 2f + unit.Radius;
		if (unit.transform.position.y <= boundaryY)
		{
			unit.ReflectVerticalMoveDirection();
			unit.transform.position = new Vector2(unit.transform.position.x, boundaryY);
		}
	}

	private void ProcessUnitWithRightBoundaryCollision(Unit unit)
	{
		float boundaryX = 0f + GameConfig.BattleConfig.gameAreaWidth / 2f - unit.Radius;
		if (unit.transform.position.x >= boundaryX)
		{
			unit.ReflectHorizontalMoveDirection();
			unit.transform.position = new Vector2(boundaryX, unit.transform.position.y);
		}
	}

	private void ProcessUnitWithLeftBoundaryCollision(Unit unit)
	{
		float boundaryX = 0f - GameConfig.BattleConfig.gameAreaWidth / 2f + unit.Radius;
		if (unit.transform.position.x <= boundaryX)
		{
			unit.ReflectHorizontalMoveDirection();
			unit.transform.position = new Vector2(boundaryX, unit.transform.position.y);
		}
	}

	private void ProcessUnitWithUnitsCollision(Unit unit)
	{
		List<Unit> neighbours = grid.GetUnitNeighbours(unit);

		for (int i = 0; i < neighbours.Count; i++)
		{
			if (unit == neighbours[i])
			{
				continue;
			}

			if (!neighbours[i].IsAlive)
			{
				continue;
			}

			if (!MathHelper.AreUnitsCollide(unit, neighbours[i]))
			{
				continue;
			}

			if (AreUnitsInOneTeam(unit, neighbours[i]))
			{
				MakeUnitsBounce(unit, neighbours[i]);
			}
			else
			{
				MakeUnitsFight(unit, neighbours[i]);
			}
		}
	}

	private bool AreUnitsInOneTeam(Unit unitA, Unit unitB)
	{
		return (unitA.TeamIndex == unitB.TeamIndex);
	}

	private void MakeUnitsBounce(Unit unitA, Unit unitB)
	{
		unitA.ChangeMoveDirection(unitA.transform.position - unitB.transform.position);
		unitB.ChangeMoveDirection(unitB.transform.position - unitA.transform.position);
	}

	private void MakeUnitsFight(Unit unitA, Unit unitB)
	{
		float distance = (unitA.transform.position - unitB.transform.position).magnitude;
		float overlapLength = unitA.Radius + unitB.Radius - distance;

		unitA.ChangeRadius(-overlapLength / 2f);
		unitB.ChangeRadius(-overlapLength / 2f);
	}
}