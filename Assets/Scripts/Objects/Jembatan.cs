using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Jembatan : TimableObject
{
    [Header("Jembatan")]
    public Animator anim;
    public List<Collider> colliders = new List<Collider>();
    Gameflow gameflow;

    void Awake()
    {
        SubsStateChange();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameflow = FindAnyObjectByType<Gameflow>();
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Player masuk jembatan");
        if (other.gameObject.CompareTag("Player") && gameflow.currentState == FlowState.PlayRoute)
        {
            // matikan semua collider dan trigger
            DisableCollider();
            // set trigger animasi
            anim = GetComponent<Animator>();
            if (anim != null)
            {
                anim.SetTrigger("Collapse");
            }
        }
    }

    void DisableCollider()
    {
        foreach (Collider collider in colliders)
        {
            collider.gameObject.SetActive(false);
        }
    }

    void EnableCollider()
    {
        foreach (Collider collider in colliders)
        {
            collider.gameObject.SetActive(true);
        }
    }
    
    override public void OnRearranged(FlowState newState)
    {
        Debug.Log($"Jembatan OnRearranged: {newState}");
        if (newState == FlowState.ArrangeRoute)
        {
            anim.SetTrigger("Reset");
            EnableCollider();
        }
    }
}
