using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class SadParticleBehavior : ParticleBehavior {

    public GameObject stickyPrefab;
    public float stickyDistance = 0.3f;

    Dictionary<GameObject, Transform> sticked;

    public override void Start() {
        base.Start();
        sticked = new Dictionary<GameObject, Transform>();
    }

    public override void Update() {
        base.Update();

        RaycastHit[] hits = Physics.SphereCastAll(transform.position, stickyDistance, transform.forward);

        foreach (RaycastHit hit in hits) {
            if (!sticked.ContainsValue(hit.collider.transform)) {
                GameObject sticky = Instantiate(stickyPrefab);
                sticky.transform.position = transform.position;
                sticky.transform.SetParent(transform);
                sticky.transform.GetChild(0).transform.position = hit.point;

                sticked.Add(sticky, hit.collider.transform);
            }
        }

        foreach (GameObject sticky in sticked.Keys) {
            if (Vector3.Distance(sticky.transform.GetChild(0).position, transform.position) > stickyDistance + 0.01f) {
                sticky.SetActive(false);
            } else {
                sticky.SetActive(true);
                sticky.transform.GetChild(0).transform.position = sticked[sticky].position;
            }
        }
    }
}