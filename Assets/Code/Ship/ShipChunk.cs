using System;
using System.Collections.Generic;
using UnityEngine;

public class ShipChunk : MonoBehaviour
{
	public ShipContainer shipContainer;

	public Rigidbody2D rigidbody2d;

	public ShipChunk()
	{
	}

	public void SetShipContainer(ShipContainer container)
	{
		this.shipContainer = container;
		base.transform.SetParent(container.transform);
		this.rigidbody2d = base.GetComponent<Rigidbody2D>();
	}

	public static List<Part> TraverseChunk(Part current, Dictionary<Part, bool> visitedParts)
	{
		if (visitedParts == null)
			visitedParts = new Dictionary<Part, bool>();
		
		List<Part> list = new List<Part>();
		list.Add(current);
		visitedParts.Add(current, true);

		foreach (Part part in current.weldedTo)
		{
			if (visitedParts.ContainsKey(part))
				continue;
			
			list.AddRange(ShipChunk.TraverseChunk(part, visitedParts));
		}
		return list;
	}
}