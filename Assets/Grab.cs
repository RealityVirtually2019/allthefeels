
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;

public class Grab : MonoBehaviour {
    #region Private Variables
    [SerializeField, Tooltip("The hand to visualize.")]
    private MLHandType _handType;

    [SerializeField, Tooltip("The GameObject to use for the Hand Center.")]
    private Transform _center;

    #endregion

    #region Private Properties
    /// <summary>
    /// Returns the hand based on the hand type.
    /// </summary>
    private MLHand Hand {
        get {
            if (_handType == MLHandType.Left) {
                return MLHands.Left;
            } else {
                return MLHands.Right;
            }
        }
    }
    #endregion

    #region Unity Methods
    /// <summary>
    /// Initializes MLHands API.
    /// </summary>
    void Start() {
        MLResult result = MLHands.Start();
        if (!result.IsOk) {
            Debug.LogErrorFormat("Error: HandTrackingVisualizer failed starting MLHands, disabling script. Reason: {0}", result);
            enabled = false;
            return;
        }

        Initialize();
    }

    /// <summary>
    /// Stops the communication to the MLHands API.
    /// </summary>
    void OnDestroy() {
        if (MLHands.IsStarted) {
            MLHands.Stop();
        }
    }

    /// <summary>
    /// Update the keypoint positions.
    /// </summary>
    void Update() {
        if (MLHands.IsStarted) {

            // Hand Center
            if (_center != null) {
                _center.position = Hand.Center;
            }

            if (Hand.IsVisible) {
                gameObject.tag = "Hand";
            } else {
                gameObject.tag = "Untagged";
            }

        }
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// Initialize the available KeyPoints.
    /// </summary>
    private void Initialize() {

    }

    #endregion
}