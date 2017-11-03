using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpaceShuttleState { Idle, Started, Moving };

public class SpaceShuttleController : MonoBehaviour
{
	public SpaceShuttleState state = SpaceShuttleState.Idle;
	public double force = 12500000;
	public double mass = 1000;
	public double height = 0;
	public double velocity = 0;
	public double acceleration = 0;
	public float time;
	public List<Engine> engines;

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
			ChangeRocketParts ();
		}
	}

	void Start ()
	{

	}

	void Update ()
	{

		switch (state)
		{
			case SpaceShuttleState.Idle:
				GetIdleInput ();
				break;
			case SpaceShuttleState.Started:
				state = SpaceShuttleState.Moving;
				break;

			case SpaceShuttleState.Moving:
				UpdateMath ();
				UpdatePosition ();
				break;
		}

	}

	private void GetIdleInput ()
	{
		if (Input.GetKeyDown (KeyCode.Space))
			state = SpaceShuttleState.Started;
	}

	private void UpdateMath ()
	{
		acceleration = force / mass;
		velocity = velocity + acceleration * Time.deltaTime;
		height = height + velocity * Time.deltaTime + 0.5 * acceleration * Time.deltaTime * Time.deltaTime;
		time += Time.deltaTime;
	}

	private void UpdatePosition ()
	{
		transform.position = Vector3.up * (float) height;
	}

	private void ChangeRocketParts ()
	{
		// manage visible rocket parts
	}

}
