using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Vector3D
{
	private double _x, _y, _z;

	public double x
	{
		get { return _x; }
		set { _x = value; }
	}
	public double y
	{
		get { return _y; }
		set { _y = value; }
	}
	public double z
	{
		get { return _z; }
		set { _z = value; }
	}
	public double magnitude
	{
		get
		{
			if (_x == 0.0 && _z == 0.0)
				return _y;
			else if (_x == 0.0 && _y == 0.0)
				return _z;
			else if (_y == 0.0 && _z == 0.0)
				return _x;
			else
				return (double) Mathf.Sqrt ((float) (_x * _x + _y * _y + _z * _z));
		}
	}
	public Vector3D normalized
	{
		get { return new Vector3D (_x / magnitude, _y / magnitude, _z / magnitude); }
	}

	public Vector3D (double x, double y)
	{
		this._x = x;
		this._y = y;
		this._z = 0.0;
	}

	public Vector3D (double x, double y, double z)
	{
		this._x = x;
		this._y = y;
		this._z = z;
	}

	public Vector3 ToVector3 ()
	{
		return new Vector3 ((float) _x, (float) _y, (float) _z);
	}

	public void Normalize ()
	{
		_x /= magnitude;
		_y /= magnitude;
		_z /= magnitude;
	}

}
