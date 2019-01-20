using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System;
using Random = UnityEngine.Random;

[Serializable]
public class MuseEvent : UnityEvent<string,float> { };

public class MuseManager : MonoBehaviour {

    public static MuseManager instance;

    private OSC osc;

    [SerializeField]
    public MuseEvent GammaEvent;
    [SerializeField]
    public MuseEvent AlphaEvent;
    [SerializeField]
    public MuseEvent BetaEvent;
    [SerializeField]
    public MuseEvent ThetaEvent;
    [SerializeField]
    public MuseEvent DeltaEvent;
    [SerializeField]
    public MuseEvent ForeheadEvent;
    [SerializeField]
    public MuseEvent ConcentrationEvent;
    [SerializeField]
    public MuseEvent MellowEvent;
    [SerializeField]
    public MuseEvent EegEvent;

    /*
delta_absolute  1-4Hz           
theta_absolute  4-8Hz           
alpha_absolute  7.5-13Hz            
beta_absolute   13-30Hz         
gamma_absolute  30-44Hz
*/

    private void Start() {
        instance = this;
    }

    void Update() {
        if (osc == null) {
            osc = FindObjectOfType<OSC>();

            if (osc != null) {
                Debug.Log("osc found, adding handlers!");
                osc.SetAddressHandler("/muse/elements/gamma_absolute", ReceiveGamma);
                osc.SetAddressHandler("/muse/elements/alpha_absolute", ReceiveAlpha);
                osc.SetAddressHandler("/muse/elements/beta_absolute", ReceiveBeta);
                osc.SetAddressHandler("/muse/elements/theta_absolute", ReceiveTheta);
                osc.SetAddressHandler("/muse/elements/delta_absolute", ReceiveDelta);
                //osc.SetAddressHandler("/muse/elements/touching_forehead", ReceiveForehead);
                //osc.SetAddressHandler("/muse/elements/algorithm/concentration", ReceiveConcentration);
                //osc.SetAddressHandler("/muse/elements/algorithm/mellow", ReceiveMellow);
                //osc.SetAddressHandler("/muse/eeg", ReceiveEeg);

            }
        }


        if (osc != null) {
            OscMessage m = new OscMessage();
            m.values = new ArrayList() { Random.Range(1f, 129f) };
            ReceiveGamma(m);

            OscMessage m2 = new OscMessage();
            m.values = new ArrayList() { Random.Range(1f, 129f) };
            ReceiveAlpha(m);

            OscMessage m3 = new OscMessage();
            m.values = new ArrayList() { Random.Range(1f, 129f) };
            ReceiveBeta(m);

            OscMessage m4 = new OscMessage();
            m.values = new ArrayList() { Random.Range(1, 129f) };
            ReceiveTheta(m);

            OscMessage m5 = new OscMessage();
            m.values = new ArrayList() { Random.Range(1f, 129f) };
            ReceiveDelta(m);
        }
    }

    public void PrintValue (string message, float min, float max) {
        Debug.Log(message);
        Debug.Log(float.Parse(message));
        Debug.Log((float.Parse(message) - min) / max);
    }

    public void ReceiveGamma(OscMessage message) {

        Debug.Log("ReceiveGamma");
        //PrintValue(message.values[0].ToString(), 30f, 44f);

        if (GammaEvent != null)
            GammaEvent.Invoke("Gamma", (float)message.values[0]);
    }

    void ReceiveAlpha(OscMessage message) {

        Debug.Log("ReceiveAlpha");
       //PrintValue(message.values[0].ToString(), 7.5f, 13f);

        if (AlphaEvent != null)
            AlphaEvent.Invoke("Alpha", (float)message.values[0]);
    }

    void ReceiveBeta(OscMessage message) {

        Debug.Log("ReceiveBeta");
        //PrintValue(message.values[0].ToString(), 13f, 30f);

        if (BetaEvent != null)
            BetaEvent.Invoke("Beta", (float)message.values[0]);
    }

    void ReceiveTheta(OscMessage message) {

        Debug.Log("ReceiveTheta");
        //PrintValue(message.values[0].ToString(), 4f, 8f);

        if (ThetaEvent != null)
            ThetaEvent.Invoke("Theta", (float)message.values[0]);
    }

    void ReceiveDelta(OscMessage message) {

        Debug.Log("ReceiveDelta");
        //PrintValue(message.values[0].ToString(), 1f, 4f);

        if (DeltaEvent != null)
            DeltaEvent.Invoke("Delta", (float)message.values[0]);
    }

    void ReceiveForehead(OscMessage message) {
        if (ForeheadEvent != null)
            ForeheadEvent.Invoke("Forehead",float.Parse(message.values[0].ToString()));
    }

    void ReceiveConcentration(OscMessage message) {
        if (ConcentrationEvent != null)
            ConcentrationEvent.Invoke("Concentration",float.Parse(message.values[0].ToString()));
    }

    void ReceiveMellow(OscMessage message) {
        if (MellowEvent != null)
            MellowEvent.Invoke("Mellow",float.Parse(message.values[0].ToString()));
    }

    void ReceiveEeg(OscMessage message) {

        Debug.Log("ReceiveEeg");
        PrintValue(message.values[0].ToString(), 0f, 1023f);

        if (EegEvent != null)
            EegEvent.Invoke("Eeg",(float.Parse(message.values[0].ToString()) - 0f) / 1023f);
    }
}
 