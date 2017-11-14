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

        if(SpaceShuttleController.isEmptyBuster)
        {

            if (this.gameObject.GetComponent<Rigidbody>() != null)
            {
                this.transform.SetParent(null);
                this.gameObject.AddComponent<Rigidbody>();
                this.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, (float)velocity, 0);
                this.gameObject.GetComponent<Rigidbody>().mass = (float)this.GetComponent<Engine>().MassTotal;
            }

        }


    }

    public static void SetHeight(double H0, double V0)
    {
        height = H0;
        velocity = V0;
    }

}
