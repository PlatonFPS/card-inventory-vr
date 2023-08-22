using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Sc_ObjectToCardManager : MonoBehaviour
{
    private Transform currentSocket = null;
    public Transform twoHandManager;
    public Transform model;
    private Sc_Interactable interactable;
    private Transform initialParent;
    public Transform trigger;
    private void Awake()
    {
        interactable = GetComponent<Sc_Interactable>();
        interactable.enabled = false;
    }
    public void SetupTriggerPosition(Transform newPosition)
    {
        trigger.position = newPosition.position;
        trigger.rotation = newPosition.rotation;
    }
    public void TryEnterZone(Transform other)
    {
        if (currentSocket == null)
        {
            currentSocket = other;
            transform.position = trigger.position;
            trigger.SetParent(transform);
            initialParent = twoHandManager.GetComponent<Sc_TwoHandManager>().initialParent;
            twoHandManager.GetComponent<Sc_TwoHandManager>().StopActivity();
            trigger.SetParent(transform);
            SetupParents();
            SetupSocketInteractable();
            RemoveInteractor();
        }
    }
    private void SetupParents()
    {
        transform.parent = twoHandManager.parent;
        twoHandManager.parent = transform;
    }
    private void SetupSocketInteractable()
    {
        GetComponent<Rigidbody>().isKinematic = false;
        model.gameObject.SetActive(true);
        MagigThing();
        interactable.enabled = true;
        interactable.initialParent = initialParent;
    }
    private void MagigThing()
    {
        twoHandManager.gameObject.SetActive(false);
        twoHandManager.gameObject.SetActive(true);
        //i don't know why, but without this this object falls very slow
    }
    private void RemoveInteractor()
    {
        if(interactable.isSelected == true)
        {
            interactable.PublicDrop();
        }
    }
    public void TryExitZone(Transform other)
    {
        if (currentSocket == other)
        {
            twoHandManager.position = trigger.position;
            initialParent = interactable.initialParent;
            RemoveSocketInteractable();
            RemoveParents();
            trigger.SetParent(twoHandManager);
            twoHandManager.GetComponent<Sc_TwoHandManager>().StartActivity();
            twoHandManager.GetComponent<Sc_TwoHandManager>().initialParent = initialParent;
            currentSocket = null;
        }
    }
    private void RemoveParents()
    {
        twoHandManager.parent = transform.parent;
        transform.parent = twoHandManager;
    }
    private void RemoveSocketInteractable()
    {
        interactable.enabled = false;
        model.gameObject.SetActive(false);
        GetComponent<Rigidbody>().isKinematic = true;
    }
}
