using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(TrackVelocity))]
public class WorldObject : MonoBehaviour {

    public Transform defaultParent;
    public float breakForce = 500f;
    public bool attached = false;
    public bool defaultGravitySetting = false;
    public GameObject attachedObject;
    bool hasRigidbody = false;

    public UnityEvent OnAttach;
    public UnityEvent OnDettach;

    private void Start() {
        defaultParent = transform.parent;

        if (OnAttach == null)
            OnAttach = new UnityEvent();

        if (OnDettach == null)
            OnDettach = new UnityEvent();

    }




    public void AttachWithJoint(Rigidbody parentRigidbody, bool forceAttach = false) {
        if (gameObject.GetComponent<FixedJoint>() == null || forceAttach) {
            if (attached && forceAttach)
                Dettach();

            gameObject.GetComponent<Rigidbody>().useGravity = false;

            FixedJoint fj = gameObject.AddComponent<FixedJoint>();
            fj.connectedBody = parentRigidbody;
            fj.breakForce = breakForce / gameObject.GetComponent<Rigidbody>().mass;

        } else {
            Debug.Log("already attached to another rigidbody");
        }
    }

    public void Attach(GameObject parentObject) {

        if (defaultParent == null)
            defaultParent = transform.parent;

        if (GetComponent<Rigidbody>()) {
            defaultGravitySetting = GetComponent<Rigidbody>().useGravity;
            hasRigidbody = true;
            GetComponent<Rigidbody>().isKinematic = true;
        }

        transform.SetParent(parentObject.transform, true);

        attachedObject = parentObject;
        attached = true;

        if (OnAttach != null)
            OnAttach.Invoke();
    }

    public void PassAttach (WorldObject newObject) {
        newObject.Attach(attachedObject);
        Dettach();
    }

    public void Dettach() {
        attached = false;
        Vector3 velocity = Vector3.zero;

        if (gameObject.transform.parent && gameObject.transform.parent != defaultParent)
            gameObject.transform.SetParent(defaultParent, true);
       
        if (hasRigidbody) {

            GetComponent<Rigidbody>().isKinematic = false;

            gameObject.GetComponent<Rigidbody>().useGravity = defaultGravitySetting;
            if (GetComponent<TrackVelocity>()) {
                Debug.Log("applying tracked velocity!");
                gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                Vector3 v = gameObject.GetComponent<TrackVelocity>().velocity;
                Debug.Log("v at time of let go " + v);
                gameObject.GetComponent<Rigidbody>().AddForce(v / gameObject.GetComponent<Rigidbody>().mass, ForceMode.VelocityChange);
                gameObject.GetComponent<TrackVelocity>().velocity = Vector3.zero;
            }
        }
        attachedObject = null;
        if (OnDettach != null)
            OnDettach.Invoke();
    }

    void OnJointBreak(float breakForce) {
        Debug.Log("A joint has just been broken!, force: " + breakForce);
        Dettach();
    }

    private void Reset() {
        if (gameObject.GetComponent<TrackVelocity>() == null) {
            gameObject.AddComponent<TrackVelocity>();
        }
    }

    private void OnCollisionEnter(Collision collision) {

    }


}
