using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipTest : MonoBehaviour {

	public PartThruster[] forward;
	public PartThruster[] backward;
	public PartThruster[] left;
	public PartThruster[] right;
	public Part breakpoint;

	public float forwardThrust;
	public float turnThrust;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		// Basic movement
		if (Input.GetKey(KeyCode.UpArrow)) {
			foreach (var item in forward)
				item.SetThrust(forwardThrust);
		} else if(Input.GetKey(KeyCode.DownArrow)) {
			foreach (var item in backward)
				item.SetThrust(forwardThrust);
		} else if(Input.GetKey(KeyCode.LeftArrow)) {
			foreach (var item in left)
				item.SetThrust(turnThrust);
		} else if(Input.GetKey(KeyCode.RightArrow)) {
			foreach (var item in right)
				item.SetThrust(turnThrust);
		} else {
			foreach (var item in right)
				item.SetThrust(0);
			foreach (var item in left)
				item.SetThrust(0);
			foreach (var item in forward)
				item.SetThrust(0);
			foreach (var item in backward)
				item.SetThrust(0);
		}
			
		// Debug break apart
		if(Input.GetKeyDown(KeyCode.Space))
			breakpoint.Detach();

		if(Input.GetKeyDown(KeyCode.V))
			breakpoint.Kill();
	}
}
