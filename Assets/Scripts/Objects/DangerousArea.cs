using UnityEngine;

public class DangerousArea : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Gameflow gameflow = FindAnyObjectByType<Gameflow>();
            if (gameflow != null) gameflow.ArrangeRoute();
        }
    }
}
