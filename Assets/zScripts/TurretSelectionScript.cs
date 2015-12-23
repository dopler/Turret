using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.CrossPlatformInput;

public class TurretSelectionScript : MonoBehaviour {

	public List<GameObject> turrets; 
	public int currentTurret;

	// Use this for initialization
	void Start () 
	{
		currentTurret = 0;
		if(turrets.Count == 0)
		{
			GameObject[] turretHolder;
			turretHolder = GameObject.FindGameObjectsWithTag("Turret");

			foreach(GameObject turret in turretHolder)
			{
				turrets.Add(turret);
			}
		}
		if(turrets.Count != 0)
		{
			turrets[currentTurret].GetComponent<TurretBehaviour>().selected = true;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(CrossPlatformInputManager.GetButtonDown("Switch"))
		{
			SwitchTurret();
		}
	}

	void SwitchTurret()
	{
		turrets[currentTurret].GetComponent<TurretBehaviour>().selected = false;
		turrets [currentTurret].GetComponent<TurretBehaviour> ().TurnOffCam ();
		currentTurret = (currentTurret + 1) % turrets.Count;
		turrets[currentTurret].GetComponent<TurretBehaviour>().selected = true;
	}

}
