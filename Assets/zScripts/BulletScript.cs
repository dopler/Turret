using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {

	public float lifeTime;

	// Use this for initialization
	void Awake()
	{
		if(lifeTime <=0)
		{
			lifeTime = 3;
		}
	}

	void Start () 
	{
		Destroy (this.gameObject, lifeTime);
	}

	void Update () 
	{
	}
}
