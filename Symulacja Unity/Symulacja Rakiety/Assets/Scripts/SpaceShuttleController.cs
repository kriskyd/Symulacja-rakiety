﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpaceShuttleState { Idle, Started, Moving };

public class SpaceShuttleController : MonoBehaviour
{
    public SpaceShuttleState state = SpaceShuttleState.Idle;
    public double force = 12500000;
    public double mass = 1000;
    public double massEngine;
    public double height = 0;
    public double velocity = 0;
    public float time = 0;
    
	public List<Engine> engines;
	public List<Engine> engineBusters;


	public FuelTank externalTank;
    public Planet planet;
    public static double G = 6.67 * Mathf.Pow(10, -11);

    public double gravity = 0;
    public double massALL = 0;
    public double massGassOut = 0;
    public double massGAssOutALL = 0;
	public double massGAssOutALLBuster = 0;
    public bool isEmpty = false;
	public bool isEmptyBuster = false;
	public float time2=0;

	//robienie z kilku silników jednego silniak
	private Engine OneEngine;
	private Engine OneEngineBusters; 
	private double newIspSL = 0;
	private double massOutBuser = 0;
	private bool isSeriouslyEmpty = false;

    [SerializeField]
    private int _MainEnginesCount;
    public int MainEnginesCount
    {
        get { return _MainEnginesCount; }
        set
        {
            _MainEnginesCount = value;
            ChangeRocketParts();
        }
    }
    [SerializeField]
    private int _SRBsCount;
    public int SRBsCount
    {
        get { return _SRBsCount; }
        set
        {
            _SRBsCount = value;
            ChangeRocketParts();
        }
    }

    void Start()
    {
		OneEngine = new Engine ();
		OneEngineBusters = new Engine ();

		OneEngine.mass = 0;
		OneEngine.fuelMass = 0;

		OneEngineBusters.mass = 0;
		OneEngineBusters.fuelMass = 0;


    }

    void Update()
    {
		
        switch (state)
        {
            case SpaceShuttleState.Idle:
                GetIdleInput();
                break;
            case SpaceShuttleState.Started:
                state = SpaceShuttleState.Moving;
                break;

            case SpaceShuttleState.Moving:
				if (!isSeriouslyEmpty)
                {
                    UpdateMath();
                    UpdatePosition();
                }
                else
                {
                    Debug.Log("koniec");
                    UpdateMathWithoutEngine();
                    UpdatePosition();
                }
                break;
        }

    }

    private void CalculateGravity(double height)
    {
        gravity = (G * planet.MASS) / ((planet.RADIUS + height) * (planet.RADIUS + height));
    }

    private void CalculateMassGassOut(Engine engine)
    {
        massGassOut = engine.thrustSL / engine.ispSL;
		newIspSL = engine.ispSL;
    }

	private void CalculateMassGassOutOnStartEngineAndBusters(List<Engine> engines, List<Engine> enginesBuster)
	{
		double licznik = 0;
		double mianownik = 0;

		foreach (Engine element in engines) {
			licznik += element.thrustSL;
			mianownik += element.thrustSL / element.ispSL;
		}

		foreach (Engine element in enginesBuster) {
			licznik += element.thrustSL;
			mianownik += element.thrustSL / element.ispSL;
		}

		newIspSL = licznik / mianownik;
			massGassOut = licznik / newIspSL;
	}

    private void GetIdleInput()
    {
		print (OneEngine.ToString ());
		Debug.Log (OneEngineBusters.ToString ());

        if (Input.GetKeyDown(KeyCode.Space))
        {
			print (OneEngine.ToString ());
			Debug.Log (OneEngineBusters.ToString ());


			//jeden silnik na dole
			foreach (Engine engine in engines)
            {

				OneEngine.mass += engine.mass;
				OneEngine.thrustSL += engine.thrustSL;
				OneEngine.thrustVac += engine.thrustVac;
            }
			OneEngine.engineType = engines [0].engineType;
			OneEngine.fuelType = engines [0].fuelType;
			OneEngine.fuelMass = externalTank.mainEngineFuelMass;


			//jeden silnk buster
			foreach(Engine buster in engineBusters)
			{
				OneEngineBusters.fuelMass += buster.fuelMass;
				OneEngineBusters.mass += buster.mass;
				OneEngineBusters.thrustSL += buster.thrustSL;
				OneEngineBusters.thrustVac += buster.thrustVac;
			}
			OneEngineBusters.engineType = engineBusters [0].engineType;
			OneEngineBusters.fuelType = engineBusters [0].fuelType;

			//oblicznie teraz IspSL  i IspVac
			CalculateIspForEngine(OneEngine, engines);
			CalculateIspForEngine (OneEngineBusters, engineBusters);


			print (OneEngine.ToString ());
			Debug.Log (OneEngineBusters.ToString ());


			//oblicznie nowego Isp oraz ile masy pozbywa się w danym momecie
			CalculateMassGassOutOnStartEngineAndBusters(engines, engineBusters);

            //obliczanie masy i promienia planety
            planet.CalculateAllMassANDRadius();

            //wyznaczenie początkowej grawitacji
            CalculateGravity(this.height);


            //całkowita masa
			massALL = mass + OneEngine.MassTotal + OneEngineBusters.MassTotal;

            state = SpaceShuttleState.Started;
        }
    }

	private void CalculateIspForEngine(Engine myEngine, List<Engine> listEngine)
	{
		double licznikIspSL = 0;
		double mianownikIsp = 0;
		double licznikIspVac = 0;
		double mianownikIspVac = 0;


		foreach (Engine engine in listEngine) {
		
			licznikIspSL += engine.thrustSL;
			licznikIspVac += engine.thrustVac;

			mianownikIsp += engine.thrustSL / engine.ispSL;
			mianownikIspVac += engine.thrustVac / engine.ispVac;
		}

		myEngine.ispSL = licznikIspSL / mianownikIsp;
		myEngine.ispVac = licznikIspVac / mianownikIspVac;
		
	}

    private void UpdateMath()
    {
		time += Time.deltaTime;
        CalculateGravity(height);

		if (isEmptyBuster == false &&  massOutBuser < OneEngineBusters.fuelMass) {

			time2 = Time.deltaTime ;

			var xmas = massGassOut * time2;
			massGAssOutALL += xmas;

			//massa pozbytego się paliwa z busterów
			massOutBuser += time2 * OneEngineBusters.thrustSL / OneEngineBusters.ispSL;

			velocity = -gravity * time2 + newIspSL * Math.Log (massALL / (massALL - massGAssOutALL));

			height = (newIspSL * time
				- 0.5 * gravity * time * time
				+ newIspSL* (time - massALL / massGassOut) * Math.Log (massALL / (massALL - massGassOut * time)));


		} else if(!isEmptyBuster){
			
			isEmptyBuster = true;

		}

		if (isEmptyBuster == false && massGAssOutALL < externalTank.mainEngineFuelMass  )
        {
			time2 = Time.deltaTime ;

			var xmas = massGassOut * time2;
			massGAssOutALL += xmas;

			velocity = -gravity * time2 + newIspSL * Math.Log (massALL / (massALL - massGAssOutALL));

			height = (newIspSL * time
				- 0.5 * gravity * time * time
				+ newIspSL* (time - massALL / massGassOut) * Math.Log (massALL / (massALL - massGassOut * time)));

        }
		else if(!isEmpty)
        {
			isEmpty = true;	
        }


		if (isEmptyBuster) {


			time2 = Time.deltaTime ;
			velocity = -gravity * time2 + newIspSL * Math.Log((massALL / (massALL - OneEngineBusters.MassTotal)));
			height = height + velocity * time2
				- 0.5 * gravity * time2 * time2
				+ newIspSL  *Math.Log ((massALL / (massALL - OneEngineBusters.MassTotal))) * time2;

			//inna masa do odrzucenia i odejmuejmy masę zrzytego paliwa
			massALL -= OneEngineBusters.MassTotal; //masa paliwa i modułu

			//trzeba policzyć na nowo prędkość wystrzeliwanego paliwa bo inny silnik
			CalculateMassGassOut (OneEngine);

			massGAssOutALL = 0;

		}

		else if (isEmptyBuster && isEmpty)
		{
			time2 = Time.deltaTime ;
			velocity = -gravity * time2 +newIspSL * Math.Log((massALL / (massALL - OneEngine.MassTotal)));
			height = height + velocity * time2
				- 0.5 * gravity * time2 * time2
				+ newIspSL  *Math.Log ((massALL / (massALL - OneEngine.MassTotal))) * time2;

			//inna masa do odrzucenia i odejmuejmy masę zrzytego paliwa
			massALL -= OneEngine.MassTotal; //masa paliwa i modułu

			massGAssOutALL = 0;

			isSeriouslyEmpty = true;
		}


    }

    public void UpdateMathWithoutEngine()
    {
        CalculateGravity(height);
		time2 = Time.deltaTime ;
		height = height + velocity * time2
			- 0.5 * gravity * time2 * time2;
		
    }

    private void UpdatePosition()
    {
        transform.position = Vector3.up * (float)height;
    }

    private void ChangeRocketParts()
    {
        // manage visible rocket parts
    }

}
