using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class AutoMuse : MonoBehaviour {

    public void startScanning() {
        Debug.Log("AutoMuse startScanning");
        // Must register at least MuseListeners before scanning for headbands.
        // Otherwise no callbacks will be triggered to get a notification.
        muse.startListening();
    }

    public void disconnect() {
        muse.disconnect();
    }

    //--------------------------------------
    // Private Members

    private string dataBuffer;
    private string connectionBuffer;
    private LibmuseBridge muse;

    //--------------------------------------
    // Private Methods

    // Use this for initialization
    void Start() {

        Debug.Log("AutoMuse Start");

#if UNITY_IPHONE
        muse = new LibmuseBridgeIos();
#elif UNITY_ANDROID || PLATFORM_LUMIN
        muse = new LibmuseBridgeAndroid();
#endif
        Debug.Log("Libmuse version = " + muse.getLibmuseVersion());

        dataBuffer = "";
        connectionBuffer = "";
        registerListeners();
        registerAllData();

        startScanning();
    }

    void registerListeners() {
        muse.registerMuseListener(this.name, "receiveMuseList");
        muse.registerConnectionListener(this.name, "receiveConnectionPackets");
        muse.registerDataListener(this.name, "receiveDataPackets");
        muse.registerArtifactListener(this.name, "receiveArtifactPackets");
    }

    void registerAllData() {
        // This will register for all the available data from muse headband
        // Comment out the ones you don't want
        muse.listenForDataPacket("ACCELEROMETER");
        muse.listenForDataPacket("GYRO");
        muse.listenForDataPacket("EEG");
        muse.listenForDataPacket("QUANTIZATION");
        muse.listenForDataPacket("BATTERY");
        muse.listenForDataPacket("DRL_REF");
        muse.listenForDataPacket("ALPHA_ABSOLUTE");
        muse.listenForDataPacket("BETA_ABSOLUTE");
        muse.listenForDataPacket("DELTA_ABSOLUTE");
        muse.listenForDataPacket("THETA_ABSOLUTE");
        muse.listenForDataPacket("GAMMA_ABSOLUTE");
        muse.listenForDataPacket("ALPHA_RELATIVE");
        muse.listenForDataPacket("BETA_RELATIVE");
        muse.listenForDataPacket("DELTA_RELATIVE");
        muse.listenForDataPacket("THETA_RELATIVE");
        muse.listenForDataPacket("GAMMA_RELATIVE");
        muse.listenForDataPacket("ALPHA_SCORE");
        muse.listenForDataPacket("BETA_SCORE");
        muse.listenForDataPacket("DELTA_SCORE");
        muse.listenForDataPacket("THETA_SCORE");
        muse.listenForDataPacket("GAMMA_SCORE");
        muse.listenForDataPacket("HSI_PRECISION");
        muse.listenForDataPacket("ARTIFACTS");
    }

    //--------------------------------------
    // These listener methods update the buffer
    // The Update() per frame will display the data.

    void receiveMuseList(string data) {
        // This method will receive a list of muses delimited by white space.
        Debug.Log("Found list of muses = " + data);

        // Convert string to list of muses and populate the dropdown menu.
        List<string> muses = data.Split(' ').ToList<string>();
        Debug.Log("AutoMuse connect");
        muse.connect(muses[0]);
    }

    void receiveConnectionPackets(string data) {
        Debug.Log("Unity received connection packet: " + data);
        connectionBuffer = data;
    }

    void receiveDataPackets(string data) {
        Debug.Log("Unity received data packet: " + data);
        dataBuffer = data;
    }

    void receiveArtifactPackets(string data) {
        Debug.Log("Unity received artifact packet: " + data);
        dataBuffer = data;
    }
}
