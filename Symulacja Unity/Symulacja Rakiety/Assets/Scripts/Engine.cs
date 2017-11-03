using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine : RocketPart
{
	public enum FuelType { liquid, solid } ;
	public enum EngineType { RD_170, RD_180, RS_25, RS_25D, SRB_4segment, SRB_5segment} ;

	public FuelType fuelType;
	public EngineType engineType;
	public double ispSL;
	public double ispVac;
	public double thrustVac;
    public double thrustSL;

	public double fuelMass;
	public double MassTotal { get { return fuelMass + mass; } }


	void Start ()
	{

	}

	void Update ()
	{

	}
}
