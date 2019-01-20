using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public enum AttractionModifier {
    CLOSEST,
    FARTHEST
}

[Serializable]
public class AttractionDefinition {
    public string tag = "";
    public float weight = 1f;
    public AttractionModifier attractionModifier;
}

public class ParticleBehavior : MonoBehaviour {

    public bool hasBecomeLeader = false;

    //amount of time it takes to move 
    public float interval;
    public Vector2 intervalRandomness;

    //amount of time between moves
    public float intervalBetween;
    public Vector2 intervalBetweenRandomness;

    //how fast does it move per interval
    public float speed;
    public Vector2 speedRandomness;

    //movement style #1
    [SerializeField]
    public AnimationCurve movementVelocity = AnimationCurve.EaseInOut(0, 0, 1, 0);

    //movement style #2
    public ForceMode forceMode;

    //physics settings
    public float drag = 0f;
    public float angulardrag = 0f;
    public float gravity = 0f;
    public float launchVelocity = 1f;

    [Range(0, 1)]
    public float AttractionWeight = 0.5f;
    [SerializeField]
    public AttractionDefinition[] attractions;

    [NonSerialized]
    public Rigidbody rigidbody;
    [NonSerialized]
    public float time = 0f;
    [NonSerialized]
    public float intervalTime = 0f;
    [NonSerialized]
    public Vector3 direction;

    public bool faceDirectionofVelocity = false;

    // Use this for initialization
    public virtual void Start() {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.drag = drag;
        rigidbody.angularDrag = angulardrag;
    }

    // Update is called once per frame
    public virtual void Update() {

        rigidbody.AddForce(new Vector3(0, gravity, 0), ForceMode.Acceleration);

        if (Time.time - time >= interval + intervalBetween) {
            intervalBetween = Random.Range(intervalBetweenRandomness.x, intervalBetweenRandomness.y);
            interval = Random.Range(intervalRandomness.x, intervalRandomness.y);
            speed = Random.Range(speedRandomness.x, speedRandomness.y);

            time = Time.time;
            intervalTime = Time.time;

            direction = CalculateTrajectory();
        }

        if (Time.time - time <= interval) {

            float delta = movementVelocity.Evaluate((Time.time - intervalTime) / interval);

            rigidbody.AddForce(direction * delta * speed, forceMode);

            if (rigidbody.velocity != Vector3.zero) {
                transform.rotation = Quaternion.LookRotation(rigidbody.velocity, Vector3.up);
            }
        }
    }

    public PointOfAttraction Nearest(string tag, bool mustbeleader) {

        PointOfAttraction[] poas = FindObjectsOfType<PointOfAttraction>();

        float distance = Mathf.Infinity;
        PointOfAttraction nearest = poas[0];

        foreach (PointOfAttraction poa in poas) {
            if (poa.tag == tag && !mustbeleader || poa.tag == tag && poa.GetComponent<ParticleBehavior>().hasBecomeLeader) {
                if (Vector3.Distance(poa.transform.position, this.transform.position) < distance) {
                    distance = Vector3.Distance(poa.transform.position, this.transform.position);
                    nearest = poa;
                }
            }
        }
        return nearest;
    }

    public Vector3 CalculateTrajectory() {

        Vector3 v = Vector3.zero;

        if (attractions != null) {
            int vcount = 0;

            List<string> ls = new List<string>();

            foreach (AttractionDefinition att in attractions) {
                if (!hasBecomeLeader || hasBecomeLeader && att.tag != this.tag) {
                    PointOfAttraction poa = Nearest(att.tag, att.tag == this.tag);
                    v += (poa.transform.position - transform.position).normalized * poa.weight * att.weight;
                    vcount++;
                }
            }

            if (vcount > 0)
                v /= vcount;
        }

        v = v.magnitude > 0
            ? v * AttractionWeight + new Vector3(UnityEngine.Random.Range(-1.00f, 1.00f), UnityEngine.Random.Range(-1.00f, 1.00f), UnityEngine.Random.Range(-1.00f, 1.00f)) * (1 - AttractionWeight)
            : new Vector3(UnityEngine.Random.Range(-1.00f, 1.00f), UnityEngine.Random.Range(-1.00f, 1.00f), UnityEngine.Random.Range(-1.00f, 1.00f));
        v = v.normalized;

        return v;
    }

    /*
    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.layer == 0) {
            transform.position = collision.contacts[0].point;
            gameObject.GetComponent<Collider>().enabled = false;
            Destroy(gameObject.GetComponent<Rigidbody>());
            enabled = false;
            transform.SetParent(collision.gameObject.transform,true);
        }
    }
    */
}