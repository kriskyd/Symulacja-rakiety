using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusterScript : RocketPart
{
    private SpaceShuttleController controller;

    public double gravity;
    public static double height;
    public static double velocity;
    public static Vector3 positionRakiet;
    private Planet planet;
    private Vector3  calulateDirection;
    float time = 0;
    float maxTime = 10f;



	// Use this for initialization
	void Start () {
        controller = FindObjectOfType<SpaceShuttleController>();
    }
	
	// Update is called once per frame
	void Update () {

        if(SpaceShuttleController.isEmptyBuster)
        {

            calulateDirection = this.gameObject.transform.GetChild(0).gameObject.transform.position - positionRakiet;

            calulateDirection.y = 0;
            calulateDirection.Normalize();



            if (this.gameObject.GetComponent<Rigidbody>() == null)
            {
                //UnityEditor.EditorApplication.isPaused = true;
                this.transform.SetParent(null);
                this.gameObject.AddComponent<Rigidbody>();
                this.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(calulateDirection.x * 30, (float)velocity *1.17f, calulateDirection.z* 30);
               // this.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(3000, 0, 0));
                
              //  this.gameObject.GetComponent<Rigidbody>().mass = (float)this.GetComponent<Engine>().fuelMass;

               
            }

             Vector3 aaa = Vector3.Lerp(gameObject.transform.eulerAngles, new Vector3(gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y, 15.0f), 0.1f);

            //this.gameObject.transform.eulerAngles = new Vector3(aaa.x, aaa.y, 360-aaa.z);

            if (time < maxTime)
            {
                time += Time.deltaTime;
                
                Debug.Log(this.gameObject.transform.eulerAngles);
            }
            else
            {
                Destroy(this.gameObject);
            }

        }


    }

    public static void SetHeight(double H0, double V0, Vector3 vector3)
    {
        height = H0;
        velocity = V0;
        positionRakiet = vector3;
    }

}
