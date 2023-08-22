using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class Sc_ClimbInteractable : XRBaseInteractable
{
    public Sc_HandAnchor anchor;
    private Transform leftHand;
    private Transform rightHand;
    private string handAnimationName;

    protected override void Awake()
    {
        base.Awake();

        leftHand = anchor.leftHandTransform;
        rightHand = anchor.rightHandTransform;
        handAnimationName = anchor.animationName;

        selectEntered.AddListener(SetupClimb);
        selectExited.AddListener(StopClimb);
    }
    protected override void OnDestroy()
    {
        selectEntered.RemoveListener(SetupClimb);
        selectExited.RemoveListener(StopClimb);
        base.OnDestroy();
    }

    public Sc_Climbing origin = null;
    public enum SocketType { Point, Axis, Plane };
    public SocketType socketType;

    private void SetupClimb(SelectEnterEventArgs arg0)
    {
        if (arg0.interactorObject.transform.name == "Right Hand Controller") origin.SetInteractor(true, handAnimationName, rightHand, socketType);
        else origin.SetInteractor(false, handAnimationName, leftHand, socketType);
    }
    private void StopClimb(SelectExitEventArgs arg0)
    {
        if (arg0.interactorObject.transform.name == "Right Hand Controller") origin.DisableInteractor(true);
        else origin.DisableInteractor(false);
    }
}
