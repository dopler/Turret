using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.CrossPlatformInput;

public class TurretBehaviour : MonoBehaviour 
{
	public GameObject xHead;
	public GameObject yHead;
	public GameObject FPV;
	public GameObject miniGun;
	public GameObject barrelEnd;
	public Rigidbody projectile;
	public List<GameObject> targets;

	public bool selected;
	public int bulletSpeed;




	public float speed;

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		if(selected)
		{
			//need a way to limit y movement of turret, otherwise could end up upside down?

			Vector3 xMoveVec = new Vector3( 0, CrossPlatformInputManager.GetAxis("Horizontal"), 0) * speed;
			Vector3 yMoveVec = new Vector3( CrossPlatformInputManager.GetAxis("Vertical") * -1, 0, 0) * (speed * .5f);
			Vector3 upDir = new Vector3(0,1,0) * 100;

			xHead.transform.Rotate(xMoveVec * Time.deltaTime);
			yHead.transform.Rotate(yMoveVec * Time.deltaTime);

			if(CrossPlatformInputManager.GetButton("Fire"))
			{
				miniGun.transform.Rotate(upDir * Time.deltaTime);

				Rigidbody clone;
				Vector3 randomDisplacement = new Vector3(Random.Range(0f,.15f),0,Random.Range(0f,.15f));
				clone = Instantiate(projectile, barrelEnd.transform.position + randomDisplacement, barrelEnd.transform.rotation) as Rigidbody;
				clone.velocity = transform.TransformDirection(barrelEnd.transform.up * bulletSpeed);

			}
			if(CrossPlatformInputManager.GetButtonDown("Zoom"))
			{
				if(FPV.activeSelf)
				{
					TurnOffCam();
				}
				else
				{
					TurnOnCam();
				}

			}

		}
		else if(targets.Count > 0)
		{
			yHead.transform.LookAt(targets[0].transform);


			Rigidbody clone;
			Vector3 randomDisplacement = new Vector3(Random.Range(0f,.15f),0,Random.Range(0f,.15f));
			clone = Instantiate(projectile, barrelEnd.transform.position + randomDisplacement, barrelEnd.transform.rotation) as Rigidbody;
			clone.velocity = transform.TransformDirection(barrelEnd.transform.up * 20);


		}
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Enemy")
		{
			targets.Add(other.gameObject);
		}
	}

	void OnTriggerExit(Collider other)
	{
		if(other.tag == "Enemy")
		{
			targets.Remove(other.gameObject);
		}
	}

	public void TurnOffCam()
	{
		FPV.SetActive (false);
	}

	public void TurnOnCam()
	{
		FPV.SetActive (true);
	}
}
