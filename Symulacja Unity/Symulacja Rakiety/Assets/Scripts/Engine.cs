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

        if (SpaceShuttleController.state == SpaceShuttleState.Started)
        {

            planet = controller.planet;
            planet.CalculateAllMassANDRadius();
        }


        if (SpaceShuttleController.isEmptyBuster && SpaceShuttleController.isEmpty)
        {

            CalculateGravity(height);
            this.transform.SetParent(null);


            velocity += gravity * Time.deltaTime;
            height += -0.5 * gravity * Time.deltaTime * Time.deltaTime - velocity * Time.deltaTime;
           

            UpdatePosition();
        }

    }

    public static void SetHeight(double H0, double V0)
    {
        height = H0;
        velocity = V0;
    }

    private void CalculateGravity(double height)
    {
        gravity = (SpaceShuttleController.G * planet.MASS) / ((planet.RADIUS + height) * (planet.RADIUS + height));
    }

    private void UpdatePosition()
    {
        transform.position = Vector3.up * (float)height;
    }

    public override string ToString ()
	{
		object[] args = new object[] {  MassTotal.ToString(), fuelMass.ToString(), mass.ToString(), ispSL.ToString(), ispVac.ToString(), thrustVac.ToString(), thrustSL.ToString()  };
		string result =  System.String.Format("[Engine: MassTotal={0}, MassFuel = {1}, mass ={2},  ispSL ={3}, ispVac = {4}, thrustVac = {5}, thrustSL= {6}]", args );
		//MassTotal.ToString(), fuelMass.ToString(), mass.ToString(), ispSL.ToString(), ispVac.ToString(), thrustVac.ToString(), thrustSL.ToString());  
		return result.ToString();
	}

}