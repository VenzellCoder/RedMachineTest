using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDependenceOnStaticEvents
{
	void SubscribeToEvents();
	void UnsubscribeFromEvents();
}
