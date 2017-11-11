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
	public UnityEngine.UI.Dropdown SRBsDropdown;
	public UnityEngine.UI.Dropdown SRBsDropdownCount;

	public GameObject enginePrefab;

	private int mec = 0;

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
		{1, "1"},
		{2, "2"},
		{3, "3"},
		{4, "4"}
	};

	void Start ()
	{
		controller = FindObjectOfType<SpaceShuttleController> ();

		// engines list
		ddMainEngines.ClearOptions ();
		ddMainEngines.AddOptions (dMainEngines.Values.ToList ());

		// engines number
		ddMainEnginesCount.ClearOptions ();
		ddMainEnginesCount.AddOptions (dMainEnginesCount.Values.ToList ());
		ddMainEnginesCount.onValueChanged.AddListener (SpawnEngines);

		// srb list
		SRBsDropdown.ClearOptions ();
		SRBsDropdown.AddOptions (dSRBs.Values.ToList ());

		//srb number
		SRBsDropdownCount.ClearOptions ();
		SRBsDropdownCount.AddOptions (dSRBsCount.Values.ToList ());


	}

	void Update ()
	{

	}

	public void SpawnEngines (int index)
	{
		mec = Convert.ToInt32 (dMainEnginesCount.Values.ElementAt (index));


		for (int i = 0; i < controller.engines.Count; i++)
			Destroy (controller.engines [i].gameObject);
		controller.engines.Clear ();

		Vector3 distVec = Vector3.forward * 4f + 2.25f * Vector3.up;
		Vector3 rotVec = Vector3.zero;
		rotVec.y = 360 / mec;
		for (int i = 0; i < mec; i++)
		{
			distVec = Quaternion.Euler (rotVec) * distVec;
			print (distVec);
			controller.engines.Add (Instantiate (enginePrefab, distVec, Quaternion.identity, controller.transform).GetComponent<Engine> ());
		}
	}
}
