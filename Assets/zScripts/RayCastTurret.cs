﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.CrossPlatformInput;

public class RayCastTurret : MonoBehaviour {

	//public GameObject xHead;
	//public GameObject yHead;
	public GameObject FPV;
	public GameObject miniGun;
	public GameObject barrelEnd;
	public GameObject indicator;
	public Rigidbody projectile;
	public GameObject missle;
	public GameObject batteryEnd;
	public List<GameObject> targets;
	
	public bool selected;
	public int bulletSpeed;
	public float missleDelay;
	
	public float speed;
	
	public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
	public float sensitivityX = 1F;
	public float sensitivityY = 1F;
	public float minimumX = -360F;
	public float maximumX = 360F;
	public float minimumY = -60F;
	public float maximumY = 60F;
	public float timeBetweenBullets = 0.15f;
	float timer;
	bool readyToFire;
	float rotationX = 0F;
	float rotationY = 0F;
	Quaternion originalRotation;
	
	
	void Start ()
	{
		// Make the rigid body not change rotation
		if (GetComponent<Rigidbody>())
			GetComponent<Rigidbody>().freezeRotation = true;
		originalRotation = transform.localRotation;
		
		readyToFire = true;
		
	}
	
	
	void Update ()
	{
		timer += Time.deltaTime;
		if(selected)
		{
			indicator.SetActive(true);
			
			
			rotationX += CrossPlatformInputManager.GetAxis("Horizontal") * sensitivityX;//Input.GetAxis("Mouse X") * sensitivityX;
			rotationY += CrossPlatformInputManager.GetAxis("Vertical") * sensitivityY;//Input.GetAxis("Mouse Y") * sensitivityY;
			rotationX = ClampAngle (rotationX, minimumX, maximumX);
			rotationY = ClampAngle (rotationY, minimumY, maximumY);
			Quaternion xQuaternion = Quaternion.AngleAxis (rotationX, Vector3.up);
			Quaternion yQuaternion = Quaternion.AngleAxis (rotationY, -Vector3.right);
			transform.localRotation = originalRotation * xQuaternion * yQuaternion;
			
			Vector3 upDir = new Vector3(0,1,0) * 100;
			
			if(CrossPlatformInputManager.GetButton("Fire") && timer > timeBetweenBullets)
			{
				miniGun.transform.Rotate(upDir * Time.deltaTime);
				barrelEnd.GetComponent<RaycastShootingScript>().Shoot();
				ShootDelay();

				if(readyToFire)
				{
					GameObject theMissle;
					theMissle = Instantiate(missle, batteryEnd.transform.position, batteryEnd.transform.rotation) as GameObject;
					if(targets.Count > 0)
					{
						theMissle.GetComponent<HomingMissleScript>().SetTarget(targets[0].gameObject);
					}
					readyToFire = false;
					StartCoroutine(MissleDelay());
				}


				
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
		else
		{
			indicator.SetActive(false);
			Vector3 upDir = new Vector3(0,1,0) * 100;
			if(targets.Count > 0)
			{
				transform.LookAt(targets[0].transform);
				
				
				miniGun.transform.Rotate(upDir * Time.deltaTime);
				if(timer > timeBetweenBullets)
				{
					miniGun.transform.Rotate(upDir * Time.deltaTime);
					barrelEnd.GetComponent<RaycastShootingScript>().Shoot();
					ShootDelay();
					
					if(readyToFire)
					{
						GameObject theMissle;
						theMissle = Instantiate(missle, batteryEnd.transform.position, batteryEnd.transform.rotation) as GameObject;
						if(targets.Count > 0)
						{
							theMissle.GetComponent<HomingMissleScript>().SetTarget(targets[0].gameObject);
						}
						readyToFire = false;
						StartCoroutine(MissleDelay());
					}
				}
			}
		}
	}

	void ShootDelay()
	{
		timer = 0f;
	}
	
	public static float ClampAngle (float angle, float min, float max)
	{
		if (angle <= -360F)
			angle += 360F;
		if (angle >= 360F)
			angle -= 360F;
		return Mathf.Clamp (angle, min, max);
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
		//originalRotation = transform.localRotation;
	}
	
	public void TurnOnCam()
	{
		FPV.SetActive (true);
	}
	
	public void ShotMissle()
	{
		
	}
	
	IEnumerator MissleDelay()
	{
		yield return new WaitForSeconds(missleDelay);
		readyToFire = true;
	}
}
