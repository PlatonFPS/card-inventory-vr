using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.XR.CoreUtils;
using UnityEngine.XR.Interaction.Toolkit;
public class Sc_CharacterFollow : MonoBehaviour
{
    private CharacterController character;
    private XROrigin rig;

    public float additionalHeadHeight { get; set; }
    private void Start()
    {
        character = GetComponent<CharacterController>();
        rig = GetComponent<XROrigin>();
    }

    private void FixedUpdate()
    {
        MatchParameters();
    }

    void MatchParameters()
    {
        character.height = rig.CameraInOriginSpaceHeight + additionalHeadHeight;
        Vector3 colliderCenter = transform.InverseTransformPoint(rig.Camera.transform.position);
        character.center = new Vector3(colliderCenter.x, character.height / 2, colliderCenter.z);
    }
}
