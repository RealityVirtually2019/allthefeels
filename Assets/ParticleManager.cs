using UnityEngine;
using System.Collections;
using System;
using Random = UnityEngine.Random;

[Serializable]
public class ParticleType {
    public GameObject prefab;
    public string tag = "";
    public int groupSize = 1;
    public int spawnCount = 1;
    public float spawnRadius = 0f;
    public AudioClip clip;
}

public class ParticleManager : MonoBehaviour {

    public GameObject spawner;

    [SerializeField]
    public ParticleType[] particles;

    Vector3 RandomCircle(Vector3 center, float radius) {
        float ang = Random.value * 360;
        Vector3 pos;
        pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.y = center.y + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
        pos.z = center.z;
        return pos;
    }

    IEnumerator ScaleUp (GameObject obj) {

        Vector3 orgScale = obj.transform.localScale;
        obj.transform.localScale = Vector3.zero;

        while (obj.transform.localScale.x < orgScale.x) {
            obj.transform.localScale += new Vector3(0.03f, 0.03f, 0.03f);
            yield return null;
        }

        obj.transform.localScale = orgScale;

    }

    // Update is called once per frame
    void Update() {
        if (Random.Range(0, 100) >= 99) {

            ParticleType pt = particles[Random.Range(0, particles.Length)];

            for (int i = 0; i < pt.spawnCount; i++) {
                GameObject p = Instantiate(pt.prefab, RandomCircle(spawner.transform.position, pt.spawnRadius), transform.rotation);
                p.GetComponent<Rigidbody>().velocity = spawner.transform.forward * p.GetComponent<ParticleBehavior>().launchVelocity;
                UpdateParticleGroupLeaders(pt);
                StartCoroutine(ScaleUp(p));
            }

            GetComponent<AudioSource>().PlayOneShot(pt.clip);

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
