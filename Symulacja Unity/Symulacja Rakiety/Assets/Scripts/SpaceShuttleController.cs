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
    public double acceleration = 0;
    public float time;
    public List<Engine> engines;
    public Planet planet;
    public static double G = 6.67 * Mathf.Pow(10, -11);

    public double gravity = 0;
    public double massALL = 0;
    public double massGassOut = 0;
    public double massGAssOutALL = 0;
    public float myTime = 0;
    public float maxTime = 4;
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
        massGassOut = engine.thrustVac / engine.ispSL;
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
            massALL = mass + massEngine;

            state = SpaceShuttleState.Started;
        }
    }

    private void UpdateMath()
    {
        CalculateGravity(height);

        if (massGAssOutALL < engines[0].fuelMass)
        {
			time2 = Time.deltaTime * 100;
			velocity = -gravity * time2 + engines[0].ispSL * Mathf.Log(2.71828f, (float)(massALL / (massALL - massGassOut * time2)));
			height = height + velocity * time2
				+ 0.5 * gravity * time2 * time2
				+ engines[0].ispSL * (1 / (-massGassOut)) * (massALL + (massALL - massGassOut * time2) * (Mathf.Log(2.71828f, (float)((massALL - massGassOut * time2) / massALL) - 1)));
			var xmas = massGassOut * time2;
                massGAssOutALL += xmas;
            massALL -= xmas;
        }
        else
        {
            Debug.Log("IIIIIIIIIIISSSSSSSSSSSSS  " + isEmpty);
            if (!isEmpty)
            {

                velocity = -gravity * Time.deltaTime + engines[0].ispSL * Mathf.Log(2.71828f, (float)(massALL / (massALL - engines[0].mass)));
                height = height + velocity * Time.deltaTime
                    + 0.5 * gravity * Time.deltaTime * Time.deltaTime
                    + engines[0].ispSL  * (massALL + (massALL - engines[0].mass) * (Mathf.Log(2.71828f, (float)((massALL - massGassOut * Time.deltaTime) / massALL) - 1)));
                isEmpty = true;
            }
            else
            {
                
                height = height + velocity * Time.deltaTime
                   + 0.5 * gravity * Time.deltaTime * Time.deltaTime;

            }

            //rakieta musi odrzucić silnik
            myTime += Time.deltaTime;
            if (myTime >= maxTime)
            {
                Debug.Log("ZZERO");

                //inna masa do odrzucenia
                massALL -= engines[0].mass;
                isEmpty = false;

                //usuawamy wykorzystany silnik
                engines.Remove(engines[0]);

                //trzeba policzyć na nowo prędkość wystrzeliwanego paliwa bo inny silnik
                CalculateMassGassOut(engines[0]);
                myTime = 0;
                massGAssOutALL = 0;


            }

        }



        time += Time.deltaTime;
    }

    public void UpdateMathWithoutEngine()
    {
        CalculateGravity(height);
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
