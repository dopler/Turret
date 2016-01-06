using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.CrossPlatformInput;

public class TurretBehaviour : MonoBehaviour 
{

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

			if(CrossPlatformInputManager.GetButton("Fire"))
			{
				miniGun.transform.Rotate(upDir * Time.deltaTime);
				
				Rigidbody clone;
				Vector3 randomDisplacement = new Vector3(Random.Range(0f,.15f),0,Random.Range(0f,.15f));
				clone = Instantiate(projectile, barrelEnd.transform.position + randomDisplacement, barrelEnd.transform.rotation) as Rigidbody;
				//clone.velocity = transform.TransformDirection(barrelEnd.transform.up * bulletSpeed);
				clone.AddForce(transform.forward * bulletSpeed);

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
				
				Rigidbody clone;
				Vector3 randomDisplacement = new Vector3(Random.Range(0f,.15f),0,Random.Range(0f,.15f));
				clone = Instantiate(projectile, barrelEnd.transform.position + randomDisplacement, barrelEnd.transform.rotation) as Rigidbody;
				//clone.velocity = transform.TransformDirection(barrelEnd.transform.up * bulletSpeed);
				clone.AddForce(transform.forward * bulletSpeed);
				if(readyToFire)
				{
					GameObject theMissle;
					theMissle = Instantiate(missle, batteryEnd.transform.position, batteryEnd.transform.rotation) as GameObject;
					theMissle.GetComponent<HomingMissleScript>().SetTarget(targets[0].gameObject);
					readyToFire = false;
					StartCoroutine(MissleDelay());
				}
			}

		}
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

	/*
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
			indicator.SetActive(true);
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
			// this makes the turret fly
			//transform.Translate(yHead.transform.forward * Time.deltaTime * speed, Space.World);
		}
		else
		{
			indicator.SetActive(false);
			if(targets.Count > 0)
			{
				yHead.transform.LookAt(targets[0].transform);


				Rigidbody clone;
				Vector3 randomDisplacement = new Vector3(Random.Range(0f,.15f),0,Random.Range(0f,.15f));
				clone = Instantiate(projectile, barrelEnd.transform.position + randomDisplacement, barrelEnd.transform.rotation) as Rigidbody;
				clone.velocity = transform.TransformDirection(barrelEnd.transform.up * 20);
			}
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


*/
	
}




















