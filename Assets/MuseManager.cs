using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System;

[Serializable]
public class MuseEvent : UnityEvent<float> { };

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

    private void Start() {
        instance = this;
    }

    void Update() {
        if (osc == null) {
            osc = FindObjectOfType<OSC>();

            if (osc != null) {
                osc.SetAddressHandler("/muse/elements/gamma_absolute", ReceiveGamma);
                osc.SetAddressHandler("/muse/elements/alpha_absolute", ReceiveAlpha);
                osc.SetAddressHandler("/muse/elements/beta_absolute", ReceiveBeta);
                osc.SetAddressHandler("/muse/elements/theta_absolute", ReceiveTheta);
                osc.SetAddressHandler("/muse/elements/delta_absolute", ReceiveDelta);
                osc.SetAddressHandler("/muse/elements/touching_forehead", ReceiveForehead);
                osc.SetAddressHandler("/muse/elements/algorithm/concentration", ReceiveConcentration);
                osc.SetAddressHandler("/muse/elements/algorithm/mellow", ReceiveMellow);
                osc.SetAddressHandler("/muse/elements/eeg", ReceiveEeg);
            }
        }
    }

    public void ReceiveGamma(OscMessage message) {
        if (GammaEvent != null)
        GammaEvent.Invoke(float.Parse(message.values[0].ToString()));
    }

    void ReceiveAlpha(OscMessage message) {
        if (AlphaEvent != null)
            AlphaEvent.Invoke(float.Parse(message.values[0].ToString()));
    }

    void ReceiveBeta(OscMessage message) {
        if (BetaEvent != null)
            BetaEvent.Invoke(float.Parse(message.values[0].ToString()));
    }

    void ReceiveTheta(OscMessage message) {
        if (ThetaEvent != null)
            ThetaEvent.Invoke(float.Parse(message.values[0].ToString()));
    }

    void ReceiveDelta(OscMessage message) {
        if (DeltaEvent != null)
            DeltaEvent.Invoke(float.Parse(message.values[0].ToString()));
    }

    void ReceiveForehead(OscMessage message) {
        if (ForeheadEvent != null)
            ForeheadEvent.Invoke(float.Parse(message.values[0].ToString()));
    }

    void ReceiveConcentration(OscMessage message) {
        if (ConcentrationEvent != null)
            ConcentrationEvent.Invoke(float.Parse(message.values[0].ToString()));
    }

    void ReceiveMellow(OscMessage message) {
        if (MellowEvent != null)
            MellowEvent.Invoke(float.Parse(message.values[0].ToString()));
    }

    void ReceiveEeg(OscMessage message) {
        if (EegEvent != null)
            EegEvent.Invoke(float.Parse(message.values[0].ToString()));
    }
}