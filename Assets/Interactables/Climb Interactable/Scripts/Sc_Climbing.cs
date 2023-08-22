using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class Sc_Climbing : MonoBehaviour
{
    private bool leftHand = false;
    private bool rightHand = false;

    private Transform interactor = null;
    private Vector3 lastPosition;

    public Transform leftTransform;
    public Transform rightTransform;

    private void Awake()
    {
        leftHand = false;
        rightHand = false;
    }

    private string handAnimationName;
    private Transform handTransform;

    public void SetInteractor(bool right, string animationName, Transform transform, Sc_ClimbInteractable.SocketType socketType = Sc_ClimbInteractable.SocketType.Point)
    {
        if(right) rightHand = true;
        else leftHand = true;
        handAnimationName = animationName;
        handTransform = transform;
        if(interactor == null)
        {
            GetComponent<ActionBasedContinuousMoveProvider>().enabled = false;
            interactor = right ? rightTransform : leftTransform;
            lastPosition = interactor.position;
        }
        (right ? rightTransform : leftTransform).GetComponent<Sc_HandReference>().SetupHand(animationName, transform, socketType);
    }
    public void DisableInteractor(bool right)
    {
        if (right) rightHand = false;
        else leftHand = false;
        if (interactor == (right ? rightTransform : leftTransform))
        {
            if(right ? leftHand : rightHand)
            {
                interactor = right ? leftTransform : rightTransform;
                lastPosition = interactor.position;
            }
            else
            {
                GetComponent<ActionBasedContinuousMoveProvider>().enabled = true;
                interactor = null;
            }
        }
        (right ? rightTransform : leftTransform).GetComponent<Sc_HandReference>().RemoveHand();
    }
    private void FixedUpdate()
    {
        if(interactor != null)
        {
            GetComponent<CharacterController>().Move(lastPosition - interactor.position);
            lastPosition = interactor.position;
        }
    }
}
