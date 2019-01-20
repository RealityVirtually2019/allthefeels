#if PLATFORM_LUMIN
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.MagicLeap;
using UnityEngine.Events;
using System;
using System.Collections;

public enum Hand {
    LEFT,
    RIGHT,
    BOTH,
    NEITHER
}

public enum HandDirection {
    BOTH,
    FRONT,
    BACK,
}

[System.Serializable]
public class GestureShowEvent : UnityEvent<MLHand> { };
[System.Serializable]
public class GestureHideEvent : UnityEvent<MLHand> { };
[System.Serializable]
public class GestureEndEvent : UnityEvent<MLHand> { };

//[RequireComponent(typeof(HandTracking))]
public class MLGesture : MonoBehaviour {

    public Hand hand;
    public MLHand handRef;
    public MLHandKeyPose Gesture;
    [Range(0.0f, 1.0f)]
    public float minimumConfidence = 0.5f;
    [Range(0.0f, 1.0f)]
    public float timeOut = 0.1f;

    public bool CancelIfControllerHand = false;
    [NonSerialized]
    public bool GestureShowing = false;
    [NonSerialized]
    public bool GestureActive = false;
    //If hand looses tracking, make sure 
    public bool ConfirmGestureEnded = true;

    public HandDirection GestureDirection;

    public GestureShowEvent GestureShow;
    public GestureHideEvent GestureHide;
    public GestureEndEvent GestureEnd;

    public virtual void Update() {
        if (!MLHands.IsStarted) {
            return;
        }

        switch (hand) {
            case Hand.LEFT:
                handRef = MLHands.Left;
                break;
            case Hand.RIGHT:
                handRef = MLHands.Right;
                break;
            case Hand.BOTH:
                //Only change if not already anchored to hand
                if (!GestureShowing) {
                    if (MLHands.Left.HandConfidence > MLHands.Right.HandConfidence && (!CancelIfControllerHand || CancelIfControllerHand && hand == Hand.LEFT)) {
                        handRef = MLHands.Left;
                    } else {
                        handRef = MLHands.Right;
                    }
                }

                break;
        }

        bool isFacingRightDirection = true;

        //Using finger and thumb position to figure out orientation of hand
        if (GestureDirection != HandDirection.BOTH) {

            Vector3 thumbPos = Camera.main.WorldToScreenPoint(handRef.Thumb.Tip.Position);
            Vector3 indexpos = Camera.main.WorldToScreenPoint(handRef.Index.Tip.Position);

            if (handRef.HandType == MLHandType.Left && thumbPos.x < indexpos.x || handRef.HandType == MLHandType.Right && thumbPos.x > indexpos.x) {
                isFacingRightDirection &= GestureDirection == HandDirection.FRONT;
            } else if (handRef.HandType == MLHandType.Left && thumbPos.x > indexpos.x || handRef.HandType == MLHandType.Right && thumbPos.x < indexpos.x) {
                isFacingRightDirection &= GestureDirection == HandDirection.BACK;
            }
        }

        bool isControllerHand = false;

        if (!isControllerHand && isFacingRightDirection && handRef.KeyPose == Gesture && handRef.KeyPoseConfidence > minimumConfidence) {
            if (!GestureShowing) {
                //Debug.Log("Gesture Detected: " + (handRef == MLHands.Left ? "Left" : "Right") + " " + Gesture.ToString());
                StopCoroutine("HideEvent");
                GestureShow.Invoke(handRef);
                GestureShowing = true;
                GestureActive = true;
            }
            return;
        } else {
            if (GestureShowing) {
                //Debug.Log("Gesture Lost: " + (handRef == MLHands.Left ? "Left" : "Right") + " " + Gesture.ToString());
                StartCoroutine("HideEvent");
                GestureShowing = false;
            }

            if (!ConfirmGestureEnded && timeOut == 0f || GestureActive && ConfirmGestureEnded && handRef.HandConfidence > minimumConfidence && handRef.KeyPose != Gesture) {
                StopCoroutine("HideEvent");
                GestureEnd.Invoke(handRef);
                GestureActive = false;
            }
        }
    }

    public IEnumerator HideEvent() {
        yield return new WaitForSeconds(timeOut);
        GestureHide.Invoke(handRef);
    }

}
#endif