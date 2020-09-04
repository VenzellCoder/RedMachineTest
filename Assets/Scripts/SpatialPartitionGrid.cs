using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Реализация паттерна пространственного разбиения:
// https://gameprogrammingpatterns.com/spatial-partition.html#entering-the-field-of-battle

public class SpatialPartitionGrid : IDependenceOnStaticEvents
{
	private const int HORIZONTAL_CELLS_AMOUNT = 10;
	private const int VERTICAL_CELLS_AMOUNT = 10;
	private float cellHeight;
	private float cellWidth;
	private List<Unit>[,] unitsInCells;


	public SpatialPartitionGrid()
	{
		CalculateCellsSize();
		SubscribeToEvents();
	}

	public void FullfilGridWithUnits(Units units)
	{
		InitializeUnitsArray();

		for (int i = 0; i < units.Count; i++)
		{
			Vector2Int posInCell = GetUnitPositionInCell(units[i].transform.position);
			unitsInCells[posInCell.x, posInCell.y].Add(units[i]);
		}
	}

	public List<Unit> GetUnitNeighbours(Unit unit)
	{
		List<Unit> neighbours = new List<Unit>();
		Vector2Int unitCell = GetUnitPositionInCell(unit.transform.position);

		// Добавить в соседи бнитов с той же клетки, что и целевой бнит
		neighbours.AddRange(unitsInCells[unitCell.x, unitCell.y]);

		// Добавить бнитов из половины*соседних клеток
		if (unitCell.x > 0 && unitCell.y > 0)
			neighbours.AddRange(unitsInCells[unitCell.x - 1, unitCell.y - 1]);

		if (unitCell.x > 0)
			neighbours.AddRange(unitsInCells[unitCell.x - 1, unitCell.y]);

		if (unitCell.y > 0)
			neighbours.AddRange(unitsInCells[unitCell.x, unitCell.y - 1]);

		if (unitCell.x > 0 && unitCell.y < VERTICAL_CELLS_AMOUNT - 1)
			neighbours.AddRange(unitsInCells[unitCell.x - 1, unitCell.y + 1]);

		neighbours.Remove(unit);
		return neighbours;
	}

	private Vector2Int GetUnitPositionInCell(Vector2 unitPos)
	{
		return GetUnitPositionInCell(unitPos.x, unitPos.y);
	}

	private Vector2Int GetUnitPositionInCell(float unitPosX, float unitPosY)
	{
		int cellX = (int)((unitPosX + GameConfig.BattleConfig.gameAreaWidth / 2) / cellWidth);
		int cellY = (int)((unitPosY + GameConfig.BattleConfig.gameAreaHeight / 2) / cellHeight);
		cellX = Mathf.Clamp(cellX, 0, VERTICAL_CELLS_AMOUNT-1);
		cellY = Mathf.Clamp(cellY, 0, HORIZONTAL_CELLS_AMOUNT-1);
		return new Vector2Int(cellX, cellY);
	}

	private void CalculateCellsSize()
	{
		cellHeight = GameConfig.BattleConfig.gameAreaHeight / HORIZONTAL_CELLS_AMOUNT;
		cellWidth = GameConfig.BattleConfig.gameAreaWidth / VERTICAL_CELLS_AMOUNT;
	}

	private void InitializeUnitsArray()
	{
		unitsInCells = new List<Unit>[VERTICAL_CELLS_AMOUNT, HORIZONTAL_CELLS_AMOUNT];

		for(int x = 0; x < VERTICAL_CELLS_AMOUNT; x++)
		{
			for(int y = 0; y < HORIZONTAL_CELLS_AMOUNT; y++)
			{
				unitsInCells[x, y] = new List<Unit>();
			}
		}
	}

	private void UpdateUnitCellPosition(Unit unit, float prevPosX, float prevPosY, float newPosX, float newPosY)
	{
		Vector2Int previousPosInCell = GetUnitPositionInCell(prevPosX, prevPosY);
		Vector2Int newPosInCell = GetUnitPositionInCell(newPosX, newPosY);

		if (previousPosInCell != newPosInCell)
		{
			unitsInCells[previousPosInCell.x, previousPosInCell.y].Remove(unit);
			unitsInCells[newPosInCell.x, newPosInCell.y].Add(unit);
		}
	}

	
	public void SubscribeToEvents()
	{
		StaticEvents.unitMoveEvent += UpdateUnitCellPosition;
	}

	public void UnsubscribeFromEvents()
	{
		StaticEvents.unitMoveEvent -= UpdateUnitCellPosition;
	}
}


