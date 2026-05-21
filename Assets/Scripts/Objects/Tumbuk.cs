using UnityEngine;

public class Tumbuk : MonoBehaviour
{
    public Animator tumbukAnim;
    private Gameflow gameflow;

    void Start()
    {
        tumbukAnim = GetComponent<Animator>();
        if (tumbukAnim == null)
        {
            Debug.LogWarning($"Animator component not found on {gameObject.name}.");
        }

        gameflow = FindAnyObjectByType<Gameflow>();
        if (gameflow != null)
        {
            gameflow.onStateChange += OnFlowStateChanged;
            OnFlowStateChanged(gameflow.currentState);
        }
        else
        {
            Debug.LogWarning("Gameflow instance not found for Tumbuk.");
        }
    }

    void OnDestroy()
    {
        if (gameflow != null)
        {
            gameflow.onStateChange -= OnFlowStateChanged;
        }
    }

    public void ActivateTumbuk()
    {
        if (tumbukAnim != null)
        {
            tumbukAnim.SetBool("Play", true);
        }
    }

    public void DeactivateTumbuk()
    {
        if (tumbukAnim != null)
        {
            tumbukAnim.SetBool("Play", false);
        }
    }

    private void OnFlowStateChanged(FlowState newState)
    {
        if (newState == FlowState.PlayRoute)
        {
            ActivateTumbuk();
        }
        else
        {
            DeactivateTumbuk();
        }
    }
}
