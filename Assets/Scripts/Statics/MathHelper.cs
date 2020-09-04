using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class MathHelper
{
	static public bool AreCirclesCollide(Vector2 pos1, float radius1, Vector2 pos2, float radius2)
	{
		float distance = (pos1 - pos2).magnitude;
		return (distance < (radius1 + radius2));
	}

	static public bool AreUnitsCollide(Unit unitA, Unit unitB)
	{
		return AreCirclesCollide(unitA.transform.position, unitA.Radius, unitB.transform.position, unitB.Radius);
	}
}
