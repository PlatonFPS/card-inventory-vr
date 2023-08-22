using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class temp : MonoBehaviour
{
    public ActionBasedController controller;
    public GameObject XrRig;
    float timeMod;
    void Start()
    {
        timeMod = Time.fixedDeltaTime;
    }
    void UpdateTime(float timeScale)
    {
        Time.timeScale = timeScale;
        Time.fixedDeltaTime *= Time.timeScale;
    }

    private float timer = 0f;
    private bool timerActive = false;
    void FixedUpdate()
    {
        if (controller.activateActionValue.action.ReadValue<float>() >= 0.3f && !timerActive)
        {
            UpdateTime(0.1f);
            XrRig.GetComponent<CharacterController>().Move(controller.transform.forward * 3f);
            timerActive = true;
        }
        if(timerActive)
        {
            timer += Time.deltaTime * 10f;
        }
        if(timer >= 3f)
        {
            UpdateTime(1f);
            timer = 0f;
            timerActive = false;
        }
    }
}
