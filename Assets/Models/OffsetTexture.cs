using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffsetTexture : MonoBehaviour {
    private SkinnedMeshRenderer renderer;
    private Vector2 offset;
	// Use this for initialization
	void Start () {
        renderer = GetComponent<SkinnedMeshRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        float perlin = Mathf.PerlinNoise(Time.time * .01f, Time.time * .01f) ;

        //offset.x += (Time.deltaTime) * .1f * perlin;
        offset.y += (Time.deltaTime) * .05f * perlin;
        renderer.material.mainTextureOffset = offset;

    }
}
