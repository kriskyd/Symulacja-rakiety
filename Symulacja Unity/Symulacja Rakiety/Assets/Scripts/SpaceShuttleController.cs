using System;
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
	public FuelTank externalTank;
    public Planet planet;
    public static double G = 6.67 * Mathf.Pow(10, -11);

    public double gravity = 0;
    public double massALL = 0;
    public double massGassOut = 0;
    public double massGAssOutALL = 0;
    public bool isEmpty = false;
	public float time2=0;

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
                if (engines.Count > 0)
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
    }

    private void GetIdleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {

            foreach (var engine in engines)
            {
                massEngine += engine.MassTotal;
            }

            //obliczanie masy i promienia planety
            planet.CalculateAllMassANDRadius();

            //wyznaczenie początkowej grawitacji
            CalculateGravity(this.height);


			//policznie predkości z jaką pozbywan ajest masa w spliku
			CalculateMassGassOut(engines[0]);

            //całkowita masa
            massALL = mass + massEngine + externalTank.MassTotal;

            state = SpaceShuttleState.Started;
        }
    }

    private void UpdateMath()
    {
		time += Time.deltaTime;
        CalculateGravity(height);

        if (massGAssOutALL < externalTank.mainEngineFuelMass)
        {
			time2 = Time.deltaTime ;

			var xmas = massGassOut * time2;
			massGAssOutALL += xmas;

			velocity = -gravity * time2 + engines [0].ispSL * Math.Log (massALL / (massALL - massGAssOutALL));



			height = -(engines [0].ispSL * time
				- 0.5 * gravity * time * time
				+ engines[0].ispSL * (time - massALL / massGassOut) * Math.Log (massALL / (massALL - massGassOut * time)));
			print (engines [0].ispSL * (time - massALL / massGassOut) * Math.Log (massALL / (massALL - massGassOut * time)));
        }
        else
        {
			isEmpty = true;

            if (isEmpty)
            {
				time2 = Time.deltaTime ;
				velocity = -gravity * time2 + engines[0].ispSL * Math.Log((massALL / (massALL - engines[0].MassTotal)));
				height = height + velocity * time2
					- 0.5 * gravity * time2 * time2
					+ engines [0].ispSL  *Math.Log ((massALL / (massALL - engines [0].MassTotal))) * time2;

				//inna masa do odrzucenia i odejmuejmy masę zrzytego paliwa
				massALL -= engines[0].MassTotal; //masa paliwa i modułu
				isEmpty = false;

				//usuawamy wykorzystany silnik
				engines.Remove(engines[0]);

				if (engines.Count > 0) {//trzeba policzyć na nowo prędkość wystrzeliwanego paliwa bo inny silnik
					CalculateMassGassOut (engines [0]);
				}
				massGAssOutALL = 0;
            }
				
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
