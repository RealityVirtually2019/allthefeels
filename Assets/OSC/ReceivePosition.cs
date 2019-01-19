using UnityEngine;
using System.Collections;

public class ReceivePosition : MonoBehaviour {
    
   	public OSC osc;
    public TextMesh text;

	// Use this for initialization
	void Start () {
	   
    }
	
	// Update is called once per frame
	void Update () {
        if (osc == null)
            osc = FindObjectOfType<OSC>();

        if (osc != null)
            osc.SetAddressHandler("/muse/elements/alpha_absolute", museRecieve);
    }

    void museRecieve (OscMessage message) {
        Debug.Log("museRecieve!");
        text.text = "Muse message recieved";
    }
}
