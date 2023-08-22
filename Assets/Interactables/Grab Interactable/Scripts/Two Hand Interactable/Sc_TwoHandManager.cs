using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Sc_TwoHandManager : MonoBehaviour
{
    public Sc_TwoHandInteractable primaryInteractable;
    public Sc_TwoHandInteractable secondaryInteractable;

    private XRBaseInteractor primaryInteractor = null;
    private XRBaseInteractor secondaryInteractor = null;

    public Transform initialParent { get; set; }
    public Transform model = null;

    public Transform trigger;

    private void Awake()
    {
        initialParent = transform.parent;
    }
    public void StopActivity()
    {
        model.gameObject.SetActive(false);
        if (primaryInteractable != null)
        {
            RemoveInteractor(Sc_TwoHandInteractable.HandType.primary);
        }
        if (secondaryInteractable != null)
        {
            RemoveInteractor(Sc_TwoHandInteractable.HandType.secondary);
        }
        primaryInteractable.gameObject.SetActive(false);
        secondaryInteractable.gameObject.SetActive(false);
        GetComponent<Rigidbody>().isKinematic = true;
    }
    public void StartActivity()
    {
        SetupPositionBasedOnTrigger();
        GetComponent<Rigidbody>().isKinematic = false;
        primaryInteractable.gameObject.SetActive(true);
        secondaryInteractable.gameObject.SetActive(true);
        model.gameObject.SetActive(true);        
    }
    private void SetupPositionBasedOnTrigger()
    {
        //nothing there yet
    }
    public bool TryToGrab(Sc_TwoHandInteractable.HandType handType, XRBaseInteractor interactor, Transform attachOffset)
    {
        if (handType == Sc_TwoHandInteractable.HandType.primary)
        {
            primaryInteractor = interactor;
            if(secondaryInteractor != null)
            {
                RemoveParents(Sc_TwoHandInteractable.HandType.secondary);
                secondaryInteractable.ForceDrop();
                FixSecondary();
            }
            SetupParents(Sc_TwoHandInteractable.HandType.primary);
            return true;
        }
        else
        {
            secondaryInteractor = interactor;
            primaryInteractable.SetupSecondaryInteractorInfo(interactor, attachOffset);
            if (primaryInteractor == null)
            {
                SetupParents(Sc_TwoHandInteractable.HandType.secondary);
                return true;
            }
            return false;
        }
    }
    public void RemoveInteractor(Sc_TwoHandInteractable.HandType handType)
    {
        if (handType == Sc_TwoHandInteractable.HandType.primary)
        {
            primaryInteractor = null;
            RemoveParents(Sc_TwoHandInteractable.HandType.primary);
            if (secondaryInteractor != null)
            {
                SetupParents(Sc_TwoHandInteractable.HandType.secondary);
                secondaryInteractable.ForceGrab();
            }
        }
        else
        {
            primaryInteractable.SetupSecondaryInteractorInfo();
            secondaryInteractor = null;
            if (primaryInteractor == null)
            {
                RemoveParents(Sc_TwoHandInteractable.HandType.secondary);
            }
            else
            {
                FixSecondary();
            }    
        }
    }
    private void FixSecondary()
    {
        secondaryInteractable.transform.SetParent(transform);
    }
    private void SetupParents(Sc_TwoHandInteractable.HandType handType)
    {
        initialParent = transform.parent;
        if (handType == Sc_TwoHandInteractable.HandType.primary)
        {
            primaryInteractable.transform.SetParent(initialParent);
            transform.SetParent(primaryInteractable.transform);
            SetTriggerParent(primaryInteractable.transform);
        }
        else
        {
            secondaryInteractable.transform.SetParent(initialParent);
            transform.SetParent(secondaryInteractable.transform);
            SetTriggerParent(secondaryInteractable.transform);
        }
        model.SetParent(transform.parent);
        GetComponent<Rigidbody>().isKinematic = true;
    }
    private void RemoveParents(Sc_TwoHandInteractable.HandType handType)
    {
        if(handType == Sc_TwoHandInteractable.HandType.primary)
        {
            transform.SetParent(initialParent);
            primaryInteractable.transform.SetParent(transform);
        }
        else
        {
            transform.SetParent(initialParent);
            secondaryInteractable.transform.SetParent(transform);
        }
        trigger.SetParent(transform);
        model.SetParent(transform);
        GetComponent<Rigidbody>().isKinematic = false;
    }
    private void SetTriggerParent(Transform parent)
    {
        trigger.SetParent(parent);
        trigger.localPosition = Vector3.zero;
        trigger.localRotation = Quaternion.identity;
    }
}
