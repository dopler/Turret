using UnityEngine;
using System.Collections;

public class HomingMissleScript : MonoBehaviour 
{
	public GameObject target;
	public float speed;

	// Use this for initialization
	void Start () 
	{
		DestroyObject (gameObject, 6);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(target != null)
		{
			transform.LookAt (target.transform);
		}
		//GetComponent<Rigidbody> ().AddForce (transform.forward * 20.0f);
		transform.Translate (Vector3.forward * Time.deltaTime * speed, Space.Self);
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Enemy")
		{
			Destroy(gameObject);
			Debug.Log("hit");
		}
		else if(other.name != "Head")
		{
			Debug.Log(other.name);
			Destroy(gameObject);
		}
	}

	public void SetTarget(GameObject destroyThis)
	{
		target = destroyThis;
	}
}
