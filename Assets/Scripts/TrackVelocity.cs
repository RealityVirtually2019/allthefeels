using UnityEngine;
using System.Collections;

public class TrackVelocity : MonoBehaviour {

    public float maxVelocity = 5f;
    public Vector3 velocity;
    public Vector3 localVelocity;
    public bool pointAt = false;

    Vector3 lastPos;

    // Use this for initialization
    void Start() {
        lastPos = transform.position;
    }

    // Update is called once per frame
    void LateUpdate() {
        velocity = (transform.position - lastPos) / Time.deltaTime;
        localVelocity = transform.InverseTransformDirection(velocity);
        lastPos = transform.position;
    }
}