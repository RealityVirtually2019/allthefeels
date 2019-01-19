using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particleSpwaner : MonoBehaviour {

    public GameObject particlePrefab;
	
	// Update is called once per frame
	void Update () {
        if (Random.Range(0, 100) >= 99) {
            GameObject p = Instantiate(particlePrefab, transform.position, transform.rotation);
            p.GetComponent<Rigidbody>().velocity = transform.forward * particlePrefab.GetComponent<ParticleBehavior>().launchVelocity;
        }
	}
}
