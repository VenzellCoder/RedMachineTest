using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutinePlayer : MonoBehaviour
{
	static public CoroutinePlayer instance;

	private void Awake()
	{
		if(instance == null)
		{
			instance = this;
		}
	}
}
