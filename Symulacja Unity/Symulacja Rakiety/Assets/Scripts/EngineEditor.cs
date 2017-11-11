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

	public GameObject engineRD_170Prefab;
	public GameObject engineRD_180Prefab;
	public GameObject engineRS_25Prefab;
	public GameObject engineRS_25DPrefab;
	public GameObject SRB4_segmentPrefab;
	public GameObject SRB5_segmentPrefab;
	public GameObject selectedEngine;
	public GameObject selectedSRB;

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
		{3, "4"}
	};

	void Start ()
	{
		controller = FindObjectOfType<SpaceShuttleController> ();

		// engines list
		ddMainEngines.ClearOptions ();
		ddMainEngines.AddOptions (dMainEngines.Values.ToList ());
		ddMainEngines.onValueChanged.AddListener (ChangeEngines);

		// engines number
		ddMainEnginesCount.ClearOptions ();
		ddMainEnginesCount.AddOptions (dMainEnginesCount.Values.ToList ());
		ddMainEnginesCount.onValueChanged.AddListener (ChangeEnginesCount);

		// srb list
		ddSRBs.ClearOptions ();
		ddSRBs.AddOptions (dSRBs.Values.ToList ());
		ddSRBs.onValueChanged.AddListener (ChangeSRBs);

		//srb number
		ddSRBsCount.ClearOptions ();
		ddSRBsCount.AddOptions (dSRBsCount.Values.ToList ());
		ddSRBsCount.onValueChanged.AddListener (ChangeSRBsCount);

		selectedEngine = engineRD_170Prefab;
		selectedSRB = SRB4_segmentPrefab;

	}

	void Update ()
	{

	}

	void UpdateEngines ()
	{
		for (int i = 0; i < controller.engines.Count; i++)
			Destroy (controller.engines [i].gameObject);
		controller.engines.Clear ();

		Vector3 distVec = Vector3.forward * 4f;
		Vector3 rotVec = Vector3.zero;
		rotVec.y = 360 / mec;
		for (int i = 0; i < mec; i++)
		{
			distVec = Quaternion.Euler (rotVec) * distVec;
			print (distVec);
			controller.engines.Add (Instantiate (selectedEngine, controller.transform.position + distVec, Quaternion.identity, controller.transform).GetComponent<Engine> ());
		}
	}

	void UpdateSRBs ()
	{
		for (int i = 0; i < controller.engineBusters.Count; i++)
			Destroy (controller.engineBusters [i].gameObject);
		controller.engineBusters.Clear ();

		Vector3 distVec = Vector3.forward * 6.5f;
		Vector3 rotVec = Vector3.zero;
		rotVec.y = srbc == 0 ? 0 : 360 / srbc;
		for (int i = 0; i < srbc; i++)
		{
			distVec = Quaternion.Euler (rotVec) * distVec;
			print (distVec);
			controller.engineBusters.Add (Instantiate (selectedSRB, controller.transform.position + distVec, Quaternion.identity, controller.transform).GetComponent<Engine> ());
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
}
