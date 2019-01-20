using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class StringParticleBehavior : ParticleBehavior {

    public Mesh[] stringMeshes;
    Color color;
    Color prevColor;

    public override void Start() {
        base.Start();
        GetComponent<MeshFilter>().sharedMesh = stringMeshes[Random.Range(0, stringMeshes.Length)];
        float f = 8.00f;
        rigidbody.angularVelocity = new Vector3(Random.Range(-f, f), Random.Range(-f, f), Random.Range(-f, f));

    }

    public override void Update() {

        if (Time.time - time >= interval + intervalBetween) {
            prevColor = GetComponent<MeshRenderer>().material.color;
            color = HueColour.HueColourValue((HueColour.HueColorNames)Random.Range(0, 10));
        }

        base.Update();

        if (Time.time - time <= interval) {
            GetComponent<MeshRenderer>().material.color = Color.Lerp(prevColor,color,(Time.time - time)/interval);
        }

        if (Random.Range(0, 100) >= 99) {

            //GetComponent<MeshFilter>().sharedMesh = stringMeshes[Random.Range(0, stringMeshes.Length)];

            float f = 8.00f;
            rigidbody.angularVelocity = new Vector3(Random.Range(-f, f), Random.Range(-f, f), Random.Range(-f, f));
        
        }
    }


}