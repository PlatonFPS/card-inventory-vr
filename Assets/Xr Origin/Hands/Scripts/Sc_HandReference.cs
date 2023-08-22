using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Sc_HandReference : MonoBehaviour
{
    private ActionBasedController controller;

    private Vector3 handInitialPosition;
    private Quaternion handInitialRotation;

    public bool HandAnimations = true;

    public GameObject hand;
    private Transform handTransform;
    private Animator handAnimator;
    public bool free = true;

    private void Start()
    {
        controller = GetComponent<ActionBasedController>();
        handTransform = hand.transform;
        handAnimator = GetComponent<Animator>();
        handInitialPosition = hand.transform.localPosition;
        handInitialRotation = hand.transform.localRotation;
    }

    private void FixedUpdate()
    {
        if(HandAnimations)
        {
            handAnimator.SetFloat("Trigger", controller.activateActionValue.action.ReadValue<float>());
            if (free)
            {
                handAnimator.SetFloat("FreeGrip", controller.selectActionValue.action.ReadValue<float>());
                handAnimator.SetFloat("FreeTrigger", controller.activateActionValue.action.ReadValue<float>());
            }
        }
    }

    private string handAnimationName;

    public void SetupHand(string handAnimationName, Transform handSocketTransform, Sc_ClimbInteractable.SocketType socketType = Sc_ClimbInteractable.SocketType.Point)
    {
        this.handAnimationName = handAnimationName;

        free = false;

        if(HandAnimations) handAnimator.SetBool(this.handAnimationName, true);

        handTransform.SetParent(handSocketTransform.transform);
        
        if (socketType == Sc_ClimbInteractable.SocketType.Point)
        {
            handTransform.localPosition = Vector3.zero;
        }
        else if(socketType == Sc_ClimbInteractable.SocketType.Axis)
        {
            handTransform.localPosition = new Vector3(0f, hand.transform.localPosition.y, 0f);
        }
        else if (socketType == Sc_ClimbInteractable.SocketType.Plane)
        {
            handTransform.localPosition = new Vector3(hand.transform.localPosition.x, hand.transform.localPosition.y, 0f);
        }

        handTransform.localPosition += handInitialPosition;
        handTransform.localRotation = handInitialRotation;
    }

    public void RemoveHand()
    {
        handTransform.SetParent(transform);
        handTransform.localPosition = handInitialPosition;
        handTransform.localRotation = handInitialRotation;

        free = true;

        if (HandAnimations) handAnimator.SetBool(handAnimationName, false);

        handAnimationName = null;
    }
}
