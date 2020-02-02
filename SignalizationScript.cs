using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class SignalizationScript : MonoBehaviour
{
    public bool isOn = false;
    public AudioSource audioSource;
    public GameObject pointLight;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setOn(bool isOn)
    {
        this.isOn = isOn;
        if (isOn)
        {
            pointLight.SetActive(true);
            audioSource.Play();
        }
        else
        {
            pointLight.SetActive(false);
            audioSource.Pause();
        }
    }
    
    
}
