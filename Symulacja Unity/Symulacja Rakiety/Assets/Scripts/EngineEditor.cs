using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EngineEditor : MonoBehaviour
{
	SpaceShuttleController controller;
	public UnityEngine.UI.Dropdown ddMainEngines;
	public UnityEngine.UI.Dropdown ddMainEnginesCount;
	public UnityEngine.UI.Dropdown ddSRBs;
	public UnityEngine.UI.Dropdown ddSRBsCount;
	public UnityEngine.UI.Dropdown ddPlanets;

	public GameObject engineRD_170Prefab;
	public GameObject engineRD_180Prefab;
	public GameObject engineRS_25Prefab;
	public GameObject engineRS_25DPrefab;
	public GameObject SRB4_segmentPrefab;
	public GameObject SRB5_segmentPrefab;
	public List<GameObject> planets;
	public GameObject selectedEngine;
	public GameObject selectedSRB;
	public GameObject selectedPlanet;

	private int mec = 1, srbc = 0;

	private readonly Dictionary<int, string> dMainEngines = new Dictionary<int, string> ()
	{
		{0, "RD_170"},
		{1, "RD_180"},
		{2, "RS_25"},
		{3, "RS_25D"}
	};

	private readonly Dictionary<int, string> dMainEnginesCount = new Dictionary<int, string> ()
	{
		{0, "1"},
		{1, "2"},
		{2, "3"},
		{3, "4"},
		{4, "5"}
	};

	private readonly Dictionary<int, string> dSRBs = new Dictionary<int, string> ()
	{
		{0, "4-segment"},
		{1, "5-segment"}
	};

	private readonly Dictionary<int, string> dSRBsCount = new Dictionary<int, string> ()
	{
		{0, "0"},
		{1, "2"},
		{2, "3"},
		{3, "4"},
		{4, "5"},
	};

	private readonly Dictionary<int, string> dPlanets = new Dictionary<int, string> ()
	{
		{0, "Hermes"},
		{1, "Afrodyta"},
		{2, "Gaja"},
		{3, "Luna"},
		{4, "Ares"},
		{5, "Zeus"},
		{6, "Kronos"},
		{7, "Uranos"},
		{8, "Posejdon"}
	};

	public List<Vector3> enginePositions;
	private List<Vector3> currentEnginesPositions = new List<Vector3> ();


	public void DoInit (SpaceShuttleController ssc)
	{
		enginePositions.Clear ();
		for (int i=0; i < ssc.engines.Count; i++)
		{
			enginePositions.Add (ssc.engines [i].transform.position);
		}

		controller = ssc;
		selectedEngine = engineRS_25Prefab;
		selectedSRB = SRB4_segmentPrefab;
		selectedPlanet = planets [2];

		// engines list
		ddMainEngines.ClearOptions ();
		ddMainEngines.AddOptions (dMainEngines.Values.ToList ());
		ddMainEngines.onValueChanged.AddListener (ChangeEngines);
		ddMainEngines.value = 2;

		// engines number
		ddMainEnginesCount.ClearOptions ();
		ddMainEnginesCount.AddOptions (dMainEnginesCount.Values.ToList ());
		ddMainEnginesCount.onValueChanged.AddListener (ChangeEnginesCount);
		ddMainEnginesCount.value = 3;

		// srb list
		ddSRBs.ClearOptions ();
		ddSRBs.AddOptions (dSRBs.Values.ToList ());
		ddSRBs.onValueChanged.AddListener (ChangeSRBs);
		ddSRBs.value = 0;

		//srb number
		ddSRBsCount.ClearOptions ();
		ddSRBsCount.AddOptions (dSRBsCount.Values.ToList ());
		ddSRBsCount.onValueChanged.AddListener (ChangeSRBsCount);
		ddSRBsCount.value = 1;

		// planets
		ddPlanets.ClearOptions ();
		ddPlanets.AddOptions (dPlanets.Values.ToList ());
		ddPlanets.onValueChanged.AddListener (ChangePlanet);
		ddPlanets.value = 2;


	}

	void Update ()
	{

	}

	void UpdateEngines ()
	{
		currentEnginesPositions.Clear ();
		switch (mec)
		{
			case 1:
				currentEnginesPositions.Add (enginePositions [2]);
				break;
			case 2:
				currentEnginesPositions.Add (enginePositions [3]);
				currentEnginesPositions.Add (enginePositions [4]);
				break;
			case 3:
				currentEnginesPositions.Add (enginePositions [2]);
				currentEnginesPositions.Add (enginePositions [3]);
				currentEnginesPositions.Add (enginePositions [4]);
				break;
			case 4:
				currentEnginesPositions.Add (enginePositions [0]);
				currentEnginesPositions.Add (enginePositions [1]);
				currentEnginesPositions.Add (enginePositions [3]);
				currentEnginesPositions.Add (enginePositions [4]);
				break;
			case 5:
				currentEnginesPositions.Add (enginePositions [0]);
				currentEnginesPositions.Add (enginePositions [1]);
				currentEnginesPositions.Add (enginePositions [2]);
				currentEnginesPositions.Add (enginePositions [3]);
				currentEnginesPositions.Add (enginePositions [4]);
				break;
		}

		if (controller)
			for (int i = 0; i < controller.engines.Count; i++)
				if (controller.engines [i] != null)
					Destroy (controller.engines [i].gameObject);
		controller.engines.Clear ();


		for (int i = 0; i < mec; i++)
		{
			controller.engines.Add (Instantiate (selectedEngine, currentEnginesPositions [i], selectedEngine.transform.rotation, controller.transform).GetComponent<Engine> ());
		}
	}

	void UpdateSRBs ()
	{
		for (int i = 0; i < controller.engineBusters.Count; i++)
			Destroy (controller.engineBusters [i].gameObject);
		controller.engineBusters.Clear ();

		if (srbc == 0)
			return;

		Vector3 rotVec = new Vector3 ();
		switch (srbc)
		{
			case 2:
				rotVec.y = 90f;
				break;
			case 3:
				rotVec.y = 75f;
				break;
			case 4:
				rotVec.y = 90f;
				break;
			case 5:
				rotVec.y = 75f;
				break;
		}

		for (int i = 0; i < srbc; i++)
		{
			controller.engineBusters.Add (Instantiate (selectedSRB, Vector3.zero, Quaternion.Euler (rotVec), controller.transform).GetComponent<Engine> ());
			switch (srbc)
			{
				case 2:
					rotVec.y += 180f;
					break;
				case 3:
					rotVec.y += 105f;
					break;
				case 4:
					rotVec.y += 60f;
					break;
				case 5:
					rotVec.y += 52.5f;
					break;
			}
		}
	}

	public void ChangeEngines (int index)
	{
		switch (dMainEngines.Values.ElementAt (index))
		{
			case "RD_170":
				selectedEngine = engineRD_170Prefab;
				break;
			case "RD_180":
				selectedEngine = engineRD_180Prefab;
				break;
			case "RS_25":
				selectedEngine = engineRS_25Prefab;
				break;
			case "RS_25D":
				selectedEngine = engineRS_25DPrefab;
				break;
		}

		UpdateEngines ();
	}

	public void ChangeSRBs (int index)
	{
		switch (dSRBs.Values.ElementAt (index))
		{
			case "4-segment":
				selectedEngine = SRB4_segmentPrefab;
				break;
			case "5-segment":
				selectedEngine = SRB5_segmentPrefab;
				break;
		}

		UpdateSRBs ();
	}

	public void ChangeEnginesCount (int index)
	{
		mec = Convert.ToInt32 (dMainEnginesCount.Values.ElementAt (index));

		UpdateEngines ();
	}

	public void ChangeSRBsCount (int index)
	{
		srbc = Convert.ToInt32 (dSRBsCount.Values.ElementAt (index));

		UpdateSRBs ();
	}

	public void ChangePlanet (int index)
	{
		selectedPlanet = planets [index];

		controller.planet = selectedPlanet.GetComponent<Planet> ();
	}

    public void ChangePlanetByOla(int index)
    {
        selectedPlanet = planets[index];

        controller.planet = selectedPlanet.GetComponent<Planet>();
        ddPlanets.value = index;

    }


}
