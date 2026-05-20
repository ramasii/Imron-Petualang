using UnityEngine;
using System.Collections.Generic;

public class TimableObject : MonoBehaviour
{
    [Header("Timable Object")]
    public int timeLine = 0;
    public List<GameObject> objectPhases = new List<GameObject>();

    protected void SubsStateChange()
    {
        Gameflow gameflow = FindAnyObjectByType<Gameflow>();
        if (gameflow)
        {
            gameflow.onStateChange += OnRearranged;
        }
    }

    public void SetPhase(int t)
    {
        timeLine = t;
        for (int i = 0; i < objectPhases.Count; i++)
        {
            if (objectPhases[i] != null)
            {
                if (i == timeLine)
                {
                    objectPhases[i].SetActive(true);
                }
                else
                {
                    objectPhases[i].SetActive(false);
                }
            }
        }
    }
    
    virtual public void OnRearranged(FlowState newState)
    {
        
    }
}
