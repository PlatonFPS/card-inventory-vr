using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sc_SocketArea : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Card")
        {
            other.gameObject.GetComponent<Sc_TriggerBridge>().EnterZone(transform);
        }    
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Card")
        {
            other.gameObject.GetComponent<Sc_TriggerBridge>().ExitZone(transform);
        }
    }
}
