using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sc_TriggerBridge : MonoBehaviour
{
    public Sc_ObjectToCardManager socketInteractable;
    public void EnterZone(Transform other)
    {
        socketInteractable.TryEnterZone(other);
    }
    public void ExitZone(Transform other)
    {
        socketInteractable.TryExitZone(other);
    }
}
