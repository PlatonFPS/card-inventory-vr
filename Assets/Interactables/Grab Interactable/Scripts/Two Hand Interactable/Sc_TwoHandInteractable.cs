using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Sc_TwoHandInteractable : Sc_Interactable
{
    protected override void Awake()
    {
        base.Awake();
        GetComponent<Rigidbody>().isKinematic = true;
    }
    protected override void Grab()
    {
        if (grabbing)
        {
            GetComponent<Rigidbody>().isKinematic = false;
            base.Grab();
            transform.SetParent(selectedInteractorObject.transform);
        }
    }
    protected override void Drop()
    {
        base.Drop();
        GetComponent<Rigidbody>().isKinematic = true;
    }

    public enum HandType { primary, secondary };
    public HandType handType;
    public HandType pitchHand;
    public Sc_TwoHandManager manager;

    protected override void SetupParent()
    {
        if(handType == HandType.primary)
        {
            base.SetupParent();
        }
    }

    private bool grabbing = false;
    protected override void BeginInteraction(SelectEnterEventArgs arg0)
    {
        if (arg0.interactorObject.transform.name == "Right Hand Controller" || arg0.interactorObject.transform.name == "Left Hand Controller")
        {
            SetupHand(arg0);
            grabbing = manager.TryToGrab(handType, arg0.interactorObject.transform.GetComponent<XRBaseInteractor>(), handTransform);
            Grab();
        }
        else if (arg0.interactorObject.transform.gameObject.GetComponent<XRSocketInteractor>() != null)
        {
            grabbing = manager.TryToGrab(handType, arg0.interactorObject.transform.GetComponent<XRBaseInteractor>(), handTransform);
            Grab();
        }
    }
    protected override void StopInteraction(SelectExitEventArgs arg0)
    {
        grabbing = false;

        Drop();
        ThrowOnDetach();

        manager.RemoveInteractor(handType);
        RemoveHand(arg0);
    }
    private void ThrowOnDetach()
    {
        Rigidbody rigidbody = manager.GetComponent<Rigidbody>();
        rigidbody.velocity = m_DetachVelocity;
        rigidbody.angularVelocity = m_DetachAngularVelocity;
    }
    public void ForceDrop()
    {
        grabbing = false;
        Drop();
    }
    public void ForceGrab()
    {
        grabbing = true;
        Grab();
    }

    private XRBaseInteractor secondaryInteractor = null;
    private Transform secondaryAttachOffset = null;

    public void SetupSecondaryInteractorInfo(XRBaseInteractor secondaryInteractor = null, Transform secondaryAttachOffset = null)
    {
        this.secondaryInteractor = secondaryInteractor;
        this.secondaryAttachOffset = secondaryAttachOffset;
    }
    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        if (grabbing)
        {
            if (selectedInteractorObject != null && secondaryInteractor != null)
            {
                selectedInteractorObject.GetComponent<XRBaseInteractor>().attachTransform.rotation = CalculateRotation();
            }
            else if (handType == HandType.primary)
            {
                selectedInteractorObject.GetComponent<XRBaseInteractor>().attachTransform.localRotation = Quaternion.identity;
            }
            base.ProcessInteractable(updatePhase);
        }
    }
    private Quaternion CalculateRotation()
    {
        Quaternion targetRotation;
        if (pitchHand == HandType.primary)
        {
            targetRotation = Quaternion.LookRotation(secondaryInteractor.attachTransform.position - (primaryInteractor.attachTransform.position + secondaryAttachOffset.localPosition), primaryInteractor.attachTransform.up);
        }
        else
        {
            targetRotation = Quaternion.LookRotation(secondaryInteractor.attachTransform.position - (primaryInteractor.attachTransform.position + secondaryAttachOffset.localPosition), secondaryInteractor.attachTransform.up);
        }
        return targetRotation;
    }
}
