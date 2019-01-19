using UnityEngine;
using System.Collections;
using System;
using Random = UnityEngine.Random;

[Serializable]
public class ParticleType {
    public GameObject prefab;
    public string tag = "";
    public int groupSize = 1;
}

public class ParticleManager : MonoBehaviour {

    public GameObject spawner;

    [SerializeField]
    public ParticleType[] particles;

    // Update is called once per frame
    void Update() {
        if (Random.Range(0, 100) >= 99) {
            ParticleType pt = particles[Random.Range(0, particles.Length)];
            GameObject p = Instantiate(pt.prefab, transform.position, transform.rotation);
            p.GetComponent<Rigidbody>().velocity = transform.forward * p.GetComponent<ParticleBehavior>().launchVelocity;
            UpdateParticleGroupLeaders(pt);
        }
    }

    public void UpdateParticleGroupLeaders (ParticleType pt) {
        GameObject[] objs = GameObject.FindGameObjectsWithTag(pt.tag);
        foreach (GameObject obj in objs) {
            obj.GetComponent<ParticleBehavior>().hasBecomeLeader = false;
        }
        for (int i = 0; i < objs.Length; i = i + pt.groupSize) {
            objs[i].GetComponent<ParticleBehavior>().hasBecomeLeader = true;
        }
    }
}
