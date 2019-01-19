using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;

public class TurnOnOSC : MonoBehaviour {

    public OSC osc;

	// Use this for initialization
	void Start () {
        MLPrivileges.Start();

    }
	
	// Update is called once per frame
	void Update () {
        if (MLPrivileges.CheckPrivilege(MLPrivilegeId.LocalAreaNetwork).Equals(MLResult.ResultOk)) {
            Debug.Log("LAN YES");
            osc.enabled = true;
            enabled = false;
        } else {
            Debug.Log("LAN NO");
        }
	}
}
