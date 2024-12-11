using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Valve : MonoBehaviour
{
    public GameObject valve;
    public GameObject valveSlot;
    public GameObject waterStream;
    public GameObject river;
    private bool isInSlot = false;
    private bool isNearPlayer = false;
    public AudioSource audioSource;

    void Start()
    {
        waterStream.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player")){
            isNearPlayer = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player")){
            isNearPlayer = false;
        }
    }

    void Update()
    {
        if (isNearPlayer && Input.GetKeyDown(KeyCode.E) && isInSlot == true)
        {
            SpinValve();
        }
    }

    public void SpinValve()
    {
        Animator valveAnimator = valve.GetComponent<Animator>();
        if (valveAnimator != null)
        {
            valveAnimator.SetTrigger("Spin"); 
        }

        Animator riverAnimator = river.GetComponent<Animator>();
        if (riverAnimator != null)
        {
            riverAnimator.SetTrigger("Rise"); 
        }
        waterStream.SetActive(true);

        PlaySound();
    }

    public void InsertValve()
    {
        // Position the valve at the slot and lock it
        valve.transform.SetParent(valveSlot.transform);
        valve.transform.localPosition = Vector3.forward * 2.0f + Vector3.up * 0.3f;
        valve.transform.localRotation = Quaternion.Euler(0, 0, 0);

        // Set it as "in slot"
        isInSlot = true;

        // Disable Rigidbody to prevent physics interactions
        GetComponent<Rigidbody>().isKinematic = true;
    }

    void PlaySound()
    {
        if (audioSource != null)
        {
            audioSource.Play(); // Play the audio
        }
    }
    
}