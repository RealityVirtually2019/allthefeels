using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceFromHeadToBlendShape : MonoBehaviour {
    public GameObject LeftHand;
    public GameObject RightHand;
    public GameObject Head;
    public float VelocityScale;
    //private VRTK.VRTK_VelocityEstimator RightVelocityComponent;
    //private VRTK.VRTK_VelocityEstimator LeftVelocityComponent;
    private SkinnedMeshRenderer skinnedMeshRenderer;
    // Use this for initialization
    void Start () {
        //RightVelocityComponent = RightHand.GetComponent<VRTK.VRTK_VelocityEstimator>();
        //LeftVelocityComponent = LeftHand.GetComponent<VRTK.VRTK_VelocityEstimator>();
        skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
    }
	
    
	// Update is called once per frame
	void Update () {
        //Vector3 RightVelocity = RightVelocityComponent.GetVelocityEstimate();
        //Vector3 LeftVelocity = LeftVelocityComponent.GetVelocityEstimate();
        Vector3 LeftRotation = Vector3.Normalize(LeftHand.transform.eulerAngles);
        Vector3 RightRotation = Vector3.Normalize(RightHand.transform.eulerAngles);
        Vector3 RightPosition = Vector3.Normalize(RightHand.transform.position);
        Vector3 LeftPosition = Vector3.Normalize(LeftHand.transform.position);
        Vector3 HeadPosition = Vector3.Normalize(Head.transform.position);
        Debug.Log("RightRotation" + RightRotation);
        skinnedMeshRenderer.SetBlendShapeWeight(0, (HeadPosition.x - LeftPosition.x ) * VelocityScale);
        skinnedMeshRenderer.SetBlendShapeWeight(1, (HeadPosition.y - LeftPosition.y) * VelocityScale);
        skinnedMeshRenderer.SetBlendShapeWeight(2, (HeadPosition.z - LeftPosition.z) * VelocityScale);
        skinnedMeshRenderer.SetBlendShapeWeight(3, (HeadPosition.x - RightPosition.x) * VelocityScale);
        skinnedMeshRenderer.SetBlendShapeWeight(4, (HeadPosition.y - RightPosition.y) * VelocityScale);
        skinnedMeshRenderer.SetBlendShapeWeight(5, (HeadPosition.z - RightPosition.z) * VelocityScale);


    }
}
