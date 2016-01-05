using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.CrossPlatformInput;


public class planeController : MonoBehaviour 
{

	public GameObject xHead;
	public GameObject yHead;
	public List<GameObject> targets;
	public int bulletSpeed;
	public float speed;
	public float turnSpeed;
	public Rigidbody planeRB;
	public float tilt;

		
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		
		//need a way to limit y movement of turret, otherwise could end up upside down?
		Vector3 xMoveVec = new Vector3( 0, CrossPlatformInputManager.GetAxis("Horizontal"), 0) * speed;
		Vector3 yMoveVec = new Vector3( CrossPlatformInputManager.GetAxis("Vertical") * -1, 0, 0) * (speed * .5f);
		//Vector3 upDir = new Vector3(0,1,0) * 100;
		
		xHead.transform.Rotate(xMoveVec * Time.deltaTime * turnSpeed);
		yHead.transform.Rotate(yMoveVec * Time.deltaTime * turnSpeed);
		
		// this makes the turret fly
		transform.Translate(yHead.transform.forward * Time.deltaTime * speed, Space.World);

		//planeRB.rotation = Quaternion.Euler (0.0f, 0.0f, CrossPlatformInputManager.GetAxis ("Horizontal") * tilt);

	}
		






	/*public int speed;
	public int turnSpeed;*/
	/*public Vector3 lift;
	public float enginePower;
	public float liftBooster;
	public Rigidbody rb;*/
	
	/*
	// Use this for initialization
	void Start () 
	{
		//rb = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		Vector3 xMoveVec = new Vector3( CrossPlatformInputManager.GetAxis("Vertical"), CrossPlatformInputManager.GetAxis("Horizontal") * -1, 0 ) * -turnSpeed;
		transform.Rotate(xMoveVec * Time.deltaTime);
		transform.Translate(this.transform.forward * Time.deltaTime * speed, Space.World);


	}*/









}
