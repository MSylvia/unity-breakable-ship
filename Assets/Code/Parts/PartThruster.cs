using System;
using UnityEngine;

public class PartThruster : Part
{
	float baseThrustForce = 1.0f;

	public float thrust;

	private float targetThrust;

	public PartThruster()
	{
	}

	private void FixedUpdate()
	{
		if (this.targetThrust > this.thrust) {
			this.thrust = Mathf.Clamp(this.thrust + 0.1f, 0f, this.targetThrust);
		} else if (this.targetThrust < this.thrust) {
			this.thrust = Mathf.Clamp(this.thrust - 0.1f, this.targetThrust, this.thrust);
		}
		base.AddForce((base.transform.up * baseThrustForce) * this.thrust);
	}

	public float GetThrust()
	{
		return this.thrust;
	}

	public override void Init(string name)
	{
		base.Init(name);
		this.type = "thruster";
		this.maxHealth = 100;
		this.health = 100;
		this.mass = 1;
	}

	public void SetThrust(float ratio)
	{
		this.targetThrust = Mathf.Clamp(ratio, 0f, 1f);
	}
}