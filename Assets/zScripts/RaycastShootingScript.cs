using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class RaycastShootingScript : MonoBehaviour {

	// Use this for initialization
	public float damagerPerShot = 20;
	public float timeBetweenBullets = 0.15f;
	public float range = 100f;
	public GameObject center;
	
	float timer;
	Ray shootRay;
	RaycastHit shootHit;
	int shootableMask;
	ParticleSystem gunParticles;
	LineRenderer gunLine;
	Light gunLight;
	float effectsDisplayTime = 0.2f;

	
	
	void Awake()
	{
		shootableMask = LayerMask.GetMask ("Shootable");
		gunParticles = GetComponent<ParticleSystem> ();
		gunLine = GetComponent<LineRenderer> ();
		gunLight = GetComponent<Light> ();
		
	}

	void Update()
	{
		timer += Time.deltaTime;

		if(CrossPlatformInputManager.GetButton("Fire") && timer > timeBetweenBullets)
		{
			//Shoot();
		}

		if(timer >= timeBetweenBullets * effectsDisplayTime)
		{
			DisableEffects();
		}
	}

	public void DisableEffects()
	{
		gunLine.enabled = false;
		gunLight.enabled = false;
	}

	public void Shoot()
	{

		// Reset the timer.
		timer = 0f;
		
		// Play the gun shot audioclip.
		//gunAudio.Play ();
		
		// Enable the light.
		gunLight.enabled = true;
		
		// Stop the particles from playing if they were, then start the particles.
		gunParticles.Stop ();
		gunParticles.Play ();
		
		// Enable the line renderer and set it's first position to be the end of the gun.
		gunLine.enabled = true;
		gunLine.SetPosition (0, transform.position);
		
		// Set the shootRay so that it starts at the end of the gun and points forward from the barrel.
		shootRay.origin = transform.position;
		shootRay.direction = center.transform.forward;
		
		// Perform the raycast against gameobjects on the shootable layer and if it hits something...
		if(Physics.Raycast (shootRay, out shootHit, range, shootableMask))
		{
			Debug.Log("here");
			
			// Set the second position of the line renderer to the point the raycast hit.
			gunLine.SetPosition (1, shootHit.point);
			Debug.Log(shootHit.point);
			Debug.Log(shootHit.transform.name);
			Debug.Log("SHOT");
		}
		// If the raycast didn't hit anything on the shootable layer...
		else
		{
			Debug.Log("no here");
			// ... set the second position of the line renderer to the fullest extent of the gun's range.
			gunLine.SetPosition (1, shootRay.origin + shootRay.direction * range);
		}
	}
}
