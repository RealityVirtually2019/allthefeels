using UnityEngine;
using System.Collections;

public class ParticleManager : MonoBehaviour {

    public int group = 3;

    // Update is called once per frame
    void Update() {
        ParticleBehavior[] pbs = FindObjectsOfType<ParticleBehavior>();
        foreach (ParticleBehavior pb in pbs) {
            pb.hasBecomeLeader = false;
        }
        for (int i = 0; i < pbs.Length; i = i + group) {
            pbs[i].hasBecomeLeader = true;
        }
    }
}
