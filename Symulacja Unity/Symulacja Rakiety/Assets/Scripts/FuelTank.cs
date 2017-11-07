using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelTank : RocketPart {

	public double mainEngineFuelMass;
	public double MassTotal { get { return mainEngineFuelMass + mass; } }

	void Start () {
		
	}
	
	void Update () {
		
	}
}
