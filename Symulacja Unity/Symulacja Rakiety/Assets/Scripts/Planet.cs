using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour {
    public double massPlanet;
    public int powerMass10;
    public double raudiusPlanet;
    public int powerRaudius10;

    public double MASS { get; private set; }
    public double RADIUS { get; private set; }



    // Use this for initialization
    void Start () {
        MASS = 0;
        RADIUS = 0;
	}


	
	// Update is called once per frame
	void Update () {
		
	}


    public void CalculateAllMassANDRadius()
    {
        MASS = massPlanet * Mathf.Pow(10, powerMass10);
        RADIUS = raudiusPlanet * Mathf.Pow(10, powerRaudius10);
    } 
}
