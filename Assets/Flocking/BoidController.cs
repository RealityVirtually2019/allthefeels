using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// these define the flock's behavior
/// </summary>
public class BoidController : MonoBehaviour
{
	public float minVelocity = 5;
	public float maxVelocity = 20;
	public float randomness = 1;
	public int flockSize = 20;
    public float distance = 1f;
    public GameObject prefab;
	public Transform target;

	internal Vector3 flockCenter;
	internal Vector3 flockVelocity;

	List<BoidFlocking> boids = new List<BoidFlocking>();

	void Start()
	{
		for (int i = 0; i < flockSize; i++)
		{
            GameObject boid = Instantiate(prefab);
            boid.transform.SetParent(transform,false);
			boid.transform.localPosition = new Vector3(
							Random.value * GetComponent<Collider>().bounds.size.x,
							Random.value * GetComponent<Collider>().bounds.size.y,
							Random.value * GetComponent<Collider>().bounds.size.z) - GetComponent<Collider>().bounds.extents;

            var flocking = boid.GetComponent<BoidFlocking>();

            flocking.controller = this;
			boids.Add(flocking);
		}
	}

	void Update()
	{
		Vector3 center = Vector3.zero;
		Vector3 velocity = Vector3.zero;
		foreach (BoidFlocking boid in boids)
		{
			center += boid.transform.localPosition;
			velocity += boid.GetComponent<Rigidbody>().velocity;
		}
		flockCenter = center / flockSize;
		flockVelocity = velocity / flockSize;
	}
}