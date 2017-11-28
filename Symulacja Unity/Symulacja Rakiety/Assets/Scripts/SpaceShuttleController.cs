using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpaceShuttleState { Idle, Started, Moving };

public class SpaceShuttleController : MonoBehaviour
{
    public static SpaceShuttleState state = SpaceShuttleState.Idle;
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
    public static bool isEmpty = false;
    public static bool isEmptyBuster = false;
    public float time2 = 0;

    //robienie z kilku silników jednego silniak
    private Engine OneEngine;
    private Engine OneEngineBusters;
    public double newIspSL = 0;
    public double massOutBuser = 0;
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

	EngineEditor editor;

    void Start()
    {
        OneEngine = new Engine();
        OneEngineBusters = new Engine();

        OneEngine.mass = 0;
        OneEngine.fuelMass = 0;

        OneEngineBusters.mass = 0;
        OneEngineBusters.fuelMass = 0;

		editor = FindObjectOfType<EngineEditor> ();
		editor.DoInit (this);
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Time.timeScale = 1f;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Time.timeScale = 2f;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Time.timeScale = 3f;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Time.timeScale = 4f;
        }

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

		if (height < 0f)
		{
			print ("Power too low!");
			//height = 0f;
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

        foreach (Engine element in engines)
        {
            licznik += element.thrustSL;
            mianownik += element.thrustSL / element.ispSL;
        }

        foreach (Engine element in enginesBuster)
        {
            licznik += element.thrustSL;
            mianownik += element.thrustSL / element.ispSL;
        }

        newIspSL = licznik / mianownik;
        massGassOut = licznik / newIspSL;
    }

    private void GetIdleInput()
    {
        //print(OneEngine.ToString());
        //Debug.Log(OneEngineBusters.ToString());

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //print(OneEngine.ToString());
            //Debug.Log(OneEngineBusters.ToString());
			editor.SetUIEnabled (false);

            //jeden silnik na dole
            foreach (Engine engine in engines)
            {

                OneEngine.mass += engine.mass;
                OneEngine.thrustSL += engine.thrustSL;
                OneEngine.thrustVac += engine.thrustVac;
            }
            OneEngine.engineType = engines[0].engineType;
            OneEngine.fuelType = engines[0].fuelType;
            OneEngine.fuelMass = externalTank.mainEngineFuelMass;


            if (engineBusters.Count == 0)
            {
                engineBusters.Add(new Engine()
                {
                    ispSL = 1,
                    ispVac = 1,
                    thrustSL = 1,
                    thrustVac = 1,
                    fuelMass = 1
                });
            }
            else
            {


                //jeden silnk buster
                foreach (Engine buster in engineBusters)
                {
                    OneEngineBusters.fuelMass += buster.fuelMass;
                    OneEngineBusters.mass += buster.mass;
                    OneEngineBusters.thrustSL += buster.thrustSL;
                    OneEngineBusters.thrustVac += buster.thrustVac;
                }
            }
            OneEngineBusters.engineType = engineBusters[0].engineType;
            OneEngineBusters.fuelType = engineBusters[0].fuelType;

            //oblicznie teraz IspSL  i IspVac
            CalculateIspForEngine(OneEngine, engines);
            CalculateIspForEngine(OneEngineBusters, engineBusters);


            //print(OneEngine.ToString());
            //Debug.Log(OneEngineBusters.ToString());


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


        foreach (Engine engine in listEngine)
        {

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

        CalculateGravity(height);

        if (isEmptyBuster == false && massOutBuser < OneEngineBusters.fuelMass)
        {

            time2 = Time.deltaTime;

            var xmas = massGassOut * time2;
            massGAssOutALL += xmas;

            //massa pozbytego się paliwa z busterów
            massOutBuser += time2 * OneEngineBusters.thrustSL / OneEngineBusters.ispSL;

            velocity = -gravity * time2 + newIspSL * Math.Log(massALL / (massALL - massGAssOutALL));

            height += newIspSL * time2 - 0.5f * gravity * time2 * time2 - gravity * time2 * time + velocity * time2 +
                +newIspSL * (time + time2 - massALL / massGassOut) * Math.Log(massALL / (massALL - massGassOut * (time2 + time)))
                - newIspSL * (time - massALL / massGassOut) * Math.Log(massALL / (massALL - massGassOut * time));

            //Debug.Log("pierwszy logarytm: " + Math.Log(massALL / (massALL - massGAssOutALL)));
            //Debug.Log("drugi logarytm: " + Math.Log(massALL / (massALL - massGassOut * time)));


        }
        else if (!isEmptyBuster)
        {


            //			time2 = Time.deltaTime ;
            //			velocity = -gravity * time2 + newIspSL * Math.Log((massALL / (massALL - OneEngineBusters.MassTotal)));
            //			height = height + velocity * time2
            //				- 0.5 * gravity * time2 * time2
            //				+ newIspSL  *Math.Log ((massALL / (massALL - OneEngineBusters.MassTotal))) * time2;

            //inna masa do odrzucenia i odejmuejmy masę zrzytego paliwa
            //massALL -= OneEngineBusters.MassTotal; //masa paliwa i modułu

            //trzeba policzyć na nowo prędkość wystrzeliwanego paliwa bo inny silnik
            CalculateMassGassOut(OneEngine);

            massGAssOutALL += OneEngineBusters.mass;

            if (massGAssOutALL < externalTank.mainEngineFuelMass + OneEngineBusters.MassTotal)
                Debug.Log("TAKK");
            BusterScript.SetHeight(height, velocity, this.gameObject.transform.position);
            Physics.gravity = new Vector3(0, (float)gravity, 0);
            isEmptyBuster = true;

        }



        if (isEmptyBuster == true && massGAssOutALL < externalTank.mainEngineFuelMass + OneEngineBusters.MassTotal)
        {
            time2 = Time.deltaTime;

            var xmas = massGassOut * time2;
            massGAssOutALL += xmas;

            velocity = -gravity * time2 + newIspSL * Math.Log(massALL / (massALL - massGAssOutALL));



            //height = (newIspSL * time
            //	- 0.5 * gravity * time * time
            //	+ newIspSL* (time - massALL / massGassOut) * Math.Log (massALL / (massALL - massGassOut * time)));
            //Debug.Log(height);

            height += newIspSL * time2 - 0.5f * gravity * time2 * time2 - gravity * time2 * time + velocity * time2 +
               +newIspSL * (time + time2 - massALL / massGassOut) * Math.Log(massALL / (massALL - massGassOut * (time2 + time)))
               - newIspSL * (time - massALL / massGassOut) * Math.Log(massALL / (massALL - massGassOut * time));


            //height = (newIspSL * time
            //                - 0.5 * gravity * time * time
            //                + newIspSL * (time - massALL / massGassOut) * Math.Log(massALL / (massALL - massGAssOutALL)));
            //Debug.Log(height);
            //Debug.Log("22222 bez busterów");
            //Debug.Log("2222 pierwszy logarytm: " + Math.Log(massALL / (massALL - massGAssOutALL)));
            //Debug.Log("2222 drugi logarytm: " + Math.Log(massALL / (massALL - massGAssOutALL)));

        }

        else if (isEmptyBuster == true && !isEmpty)
        {
            time2 = Time.deltaTime;
            //			velocity = -gravity * time2 +newIspSL * Math.Log((massALL / (massALL - OneEngine.MassTotal)));
            //			height = height + velocity * time2
            //				- 0.5 * gravity * time2 * time2
            //				+ newIspSL  *Math.Log ((massALL / (massALL - OneEngine.MassTotal))) * time2;

            //inna masa do odrzucenia i odejmuejmy masę zrzytego paliwa
            //massALL -= OneEngine.MassTotal; //masa paliwa i modułu

            massGAssOutALL = 0;


            isEmpty = true;

            isSeriouslyEmpty = true;
            Engine.SetHeight(height, velocity);
            Physics.gravity = new Vector3(0, (float)gravity, 0);
        }

        time += Time.deltaTime;


    }

    public void UpdateMathWithoutEngine()
    {
        CalculateGravity(height);
        time2 = Time.deltaTime;
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
