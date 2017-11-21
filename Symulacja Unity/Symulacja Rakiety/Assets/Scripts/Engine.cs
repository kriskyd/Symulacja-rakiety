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

    public double gravity;
    public static double height;
    public static double velocity;
    private Planet planet;
    private SpaceShuttleController controller;



    void Start ()
	{

        controller = FindObjectOfType<SpaceShuttleController>();

        if (ispVac == 0)
			ispVac = ispSL;
		if (thrustVac == 0)
			thrustVac = thrustSL;
	}

	void Update ()
	{


        if (SpaceShuttleController.isEmptyBuster && SpaceShuttleController.isEmpty)
        {
           
            if (this.gameObject.GetComponent<Rigidbody>() == null)
            {
                this.transform.SetParent(null);
                this.gameObject.AddComponent<Rigidbody>();
                this.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, (float)velocity, 0);
                this.gameObject.GetComponent<Rigidbody>().mass = (float)this.MassTotal;
            }

        }

    }

    public static void SetHeight(double H0, double V0)
    {
        height = H0;
        velocity = V0;
    }


    public override string ToString ()
	{
		object[] args = new object[] {  MassTotal.ToString(), fuelMass.ToString(), mass.ToString(), ispSL.ToString(), ispVac.ToString(), thrustVac.ToString(), thrustSL.ToString()  };
		string result =  System.String.Format("[Engine: MassTotal={0}, MassFuel = {1}, mass ={2},  ispSL ={3}, ispVac = {4}, thrustVac = {5}, thrustSL= {6}]", args );
		//MassTotal.ToString(), fuelMass.ToString(), mass.ToString(), ispSL.ToString(), ispVac.ToString(), thrustVac.ToString(), thrustSL.ToString());  
		return result.ToString();
	}

}