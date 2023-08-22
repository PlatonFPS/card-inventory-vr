using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class Sc_HandAnchor : MonoBehaviour
{
    public string animationName = "Empty";

    public bool leftHand = false;
    public Transform leftHandTransform = null;

    public bool rightHand = false;
    public Transform rightHandTransform = null;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Right Hand Controller")
        {
            rightHand = true;
        }
        else
        {
            leftHand = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Right Hand Controller")
        {
            rightHand = false;
        }
        else
        {
            leftHand = false;
        }
    }
}
