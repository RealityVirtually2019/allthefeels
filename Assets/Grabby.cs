using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class OnGrabEvent : UnityEvent<GameObject> { };

public class Grabby : MonoBehaviour {

    public WorldObject worldObject;
    public bool TriggerDown = false;
    public float CollisionSensitivityTime = 0.2f;
    Coroutine CollisionExitDelayCoroutine;

    public OnGrabEvent OnAttach;
    public OnGrabEvent OnDettach;



    private void OnTriggerEnter(Collider other) {
        Debug.Log("triggggered");
        if (!TriggerDown) {
            Debug.Log("ahahaa " + other.gameObject.name);
            if (other.gameObject.GetComponent<WorldObject>()) {
                Debug.Log("eeeee");
                if (!worldObject || !worldObject.attached) {
                    Debug.Log("world obj");
                    worldObject = other.gameObject.GetComponent<WorldObject>();

                    if (CollisionExitDelayCoroutine != null)
                        StopCoroutine(CollisionExitDelayCoroutine);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (worldObject && !worldObject.attached) {
            CollisionExitDelayCoroutine = StartCoroutine(CollisionExitDelay());
        }
    }

    IEnumerator CollisionExitDelay() {
        yield return new WaitForSeconds(CollisionSensitivityTime);
        worldObject = null;
    }

    void StopCollisionExitDelay () {
        if (CollisionExitDelayCoroutine != null)
            StopCoroutine(CollisionExitDelayCoroutine);
    }

    private void Update() {
        if (worldObject != null) {
            if (!worldObject.attached && TriggerDown) {

                StopCollisionExitDelay();

                worldObject.Attach(gameObject);

                if (OnAttach != null)
                    OnAttach.Invoke(worldObject.gameObject);

            } else if (worldObject.attached && !TriggerDown) {

                worldObject.Dettach();

                if (OnDettach != null)
                    OnDettach.Invoke(worldObject.gameObject);

                worldObject = null;
            } 
        }
    }

    public void InputBegan () {
        TriggerDown = true;

        var p = GetComponent<ParticleSystem>().main;
        p.startSize = 0.5f;
    }

    public void InputEnded () {
        TriggerDown = false;

        var p = GetComponent<ParticleSystem>().main;
        p.startSize = 0.15f;

        if (CollisionExitDelayCoroutine != null)
            StopCoroutine(CollisionExitDelayCoroutine);
    }
}
