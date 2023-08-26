using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Sc_HandSwitchTest : MonoBehaviour
{
    private XRGrabInteractable interactable;

    private void Awake()
    {
        interactable = GetComponent<XRGrabInteractable>();
        interactable.selectEntered.AddListener(Pon);
    }
    private void Pon(SelectEnterEventArgs arg0)
    {
        Debug.Log(arg0.interactorObject.transform.name);
    }
    public void SwitchAttachTransforms()
    {
        Transform temporaryTransform = interactable.attachTransform;
        interactable.attachTransform = interactable.secondaryAttachTransform;
        interactable.secondaryAttachTransform = temporaryTransform;
    }
    
}
