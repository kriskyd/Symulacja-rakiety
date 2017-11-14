using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusterScript : RocketPart
{
    private SpaceShuttleController controller;

    public double gravity;
    public static double height;
    public static double velocity;
    private Planet planet;



	// Use this for initialization
	void Start () {
        controller = FindObjectOfType<SpaceShuttleController>();
    }
	
	// Update is called once per frame
	void Update () {

        if(SpaceShuttleController.state == SpaceShuttleState.Started)
        {
            
            planet = controller.planet;
            planet.CalculateAllMassANDRadius();
        }


        if(SpaceShuttleController.isEmptyBuster)
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
}
