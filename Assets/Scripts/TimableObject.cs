using UnityEngine;
using System.Collections.Generic;

public class TimableObject : MonoBehaviour
{
    public int timeLine = 0;
    public List<GameObject> objectPhases = new List<GameObject>();

    public void SetPhase(int t)
    {
        timeLine = t;
        for (int i = 0; i < objectPhases.Count; i++)
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
