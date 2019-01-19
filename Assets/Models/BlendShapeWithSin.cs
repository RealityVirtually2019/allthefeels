using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlendShapeWithSin : MonoBehaviour {
    private SkinnedMeshRenderer skinnedMeshRenderer;
    public List<float> speed;
    public float scale = 100;

    // Use this for initialization
    void Start () {
        skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
    }
	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < speed.Count; i++)
        {
            if (speed[i] > 0f)
            {
                float val = Mathf.PerlinNoise(Time.time * speed[i], Time.time * speed[i]);
                Debug.Log("perlin" + val);
                //Debug.Log("scale" + Time.time * scale[i]);
                skinnedMeshRenderer.SetBlendShapeWeight(i, val * 10);

            }

        }
        

    }
}
