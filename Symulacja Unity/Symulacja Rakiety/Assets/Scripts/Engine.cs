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

		if (ispVac == 0)
			ispVac = ispSL;
		if (thrustVac == 0)
			thrustVac = thrustSL;
	}

	void Update ()
	{

	}

	 public override string ToString ()
	{
		object[] args = new object[] {  MassTotal.ToString(), fuelMass.ToString(), mass.ToString(), ispSL.ToString(), ispVac.ToString(), thrustVac.ToString(), thrustSL.ToString()  };
		string result =  System.String.Format("[Engine: MassTotal={0}, MassFuel = {1}, mass ={2},  ispSL ={3}, ispVac = {4}, thrustVac = {5}, thrustSL= {6}]", args );
		//MassTotal.ToString(), fuelMass.ToString(), mass.ToString(), ispSL.ToString(), ispVac.ToString(), thrustVac.ToString(), thrustSL.ToString());  
		return result.ToString();
	}

//	public override string ToString ()
//	{
//
//		string result =  "[Engine: MassTotal=" + MassTotal.ToString() + ", MassFuel = " + fuelMass.ToString() +
//			" mass = " + mass.ToString() + "ispSL = "+ ispSL.ToString()+ " ispVac = " + ispVac.ToString() + 
//			"thrustVac = " + thrustVac.ToString() +" thrustSL= " + thrustSL.ToString() + "]";
//		//MassTotal.ToString(), fuelMass.ToString(), mass.ToString(), ispSL.ToString(), ispVac.ToString(), thrustVac.ToString(), thrustSL.ToString());  
//
//		return result.ToString();
//	}
}