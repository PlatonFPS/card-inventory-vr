using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Sc_Interactable : XRGrabInteractable
{
    private Transform socketAttachTransform;
    private List<Sc_HandAnchor> handAnchors = new List<Sc_HandAnchor>();
    public Transform initialParent { get; set; }

    protected override void Awake()
    {
        base.Awake();

        initialParent = transform.parent;

        SetupAnchors();

        selectEntered.AddListener(BeginInteraction);
        selectExited.AddListener(StopInteraction);
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();

        selectEntered.RemoveListener(BeginInteraction);
        selectExited.RemoveListener(StopInteraction);
    }
    private void SetupAnchors()
    {
        for (int i = 0; i < colliders.Count; i++)
        {
            handAnchors.Add(colliders[i].GetComponent<Sc_HandAnchor>());
        }
    }
    protected virtual void BeginInteraction(SelectEnterEventArgs arg0)
    {
        SetupHand(arg0);
    }
    protected virtual void StopInteraction(SelectExitEventArgs arg0)
    {
        RemoveHand(arg0);
    }

    public void PublicDrop()
    {
        RemoveHand(null);
        Drop();
    }

    protected XRBaseInteractor primaryInteractor = null;

    protected GameObject selectedInteractorObject = null;
    private Quaternion interactorInitialLocalRotation;
    protected Transform handTransform = null;
    private string handAnimationName = null;

    protected virtual void SetupHand(SelectEnterEventArgs arg0)
    {
        SetupInteractorInfo(arg0);

        SetupParent();

        TryToSetupTriggerPosition();

        SetupHandInfo();
        attachTransform = handTransform;

        selectedInteractorObject.GetComponent<Sc_HandReference>().SetupHand(handAnimationName, handTransform);
    }
    protected virtual void SetupParent() { transform.parent = selectedInteractorObject.transform; }
    private void TryToSetupTriggerPosition()
    {

    }
    private void SetupInteractorInfo(SelectEnterEventArgs arg0)
    {
        selectedInteractorObject = arg0.interactorObject.transform.gameObject;
        primaryInteractor = selectedInteractorObject.GetComponent<XRBaseInteractor>();
        interactorInitialLocalRotation = primaryInteractor.attachTransform.localRotation;
    }
    private void SetupHandInfo()
    {
        if (selectedInteractorObject.name == "Right Hand Controller" ? true : false)
        {
            for (int i = 0; i < handAnchors.Count; i++)
            {
                if (handAnchors[i].rightHand)
                {
                    handAnimationName = handAnchors[i].animationName;
                    handTransform = handAnchors[i].rightHandTransform;
                }
            }
        }
        else
        {
            for (int i = 0; i < handAnchors.Count; i++)
            {
                if (handAnchors[i].leftHand)
                {
                    handAnimationName = handAnchors[i].animationName;
                    handTransform = handAnchors[i].leftHandTransform;
                }
            }
        }
    }
    protected virtual void RemoveHand(SelectExitEventArgs arg0)
    {
        if (selectedInteractorObject != null)
        {
            selectedInteractorObject.GetComponent<Sc_HandReference>().RemoveHand();

            DeleteHandInfo();
            attachTransform = handTransform;

            transform.parent = initialParent;

            DeleteInteractorInfo();
        }
    }
    protected virtual void RemoveParent() { transform.parent = initialParent; }
    private void DeleteInteractorInfo()
    {
        primaryInteractor = null;
        selectedInteractorObject = null;
    }
    private void DeleteHandInfo()
    {
        handAnimationName = null;
        handTransform = null;
    }
}

