using System;
using System.Collections.Generic;
using UnityEngine;

public class ShipContainer : MonoBehaviour
{
	public List<Part> parts;

	private float lastUpdateTimer;

	public ShipContainer()
	{
	}

	public void Awake () {
		// Add all parts
		parts.AddRange(this.GetComponentsInChildren<Part>());
	}

	public void KillShip()
	{
		while (this.parts.Count > 0) {
			this.parts[0].Kill();
		}
	}

	private void OnDestroy()
	{
	
	}

	public void PartDamaged(float amount)
	{
		
	}

	public void PartKilled()
	{
		if (this.parts.Count <= 0) {
		}
	}
}