using UnityEngine;
using Unity.Cinemachine;
using System.Collections.Generic;

public enum FlowState
{
    ArrangeRoute,
    PlayRoute
}

public class Gameflow : MonoBehaviour
{
    public FlowState currentState = FlowState.ArrangeRoute;
    [Tooltip("Index [0] untuk ArrangeRoute, Index [1] untuk PlayRoute")]
    public List<CinemachineCamera> cineCams;
    public delegate void OnStateChange(FlowState newState);
    public event OnStateChange onStateChange;

    public void PlayRoute()
    {
        currentState = FlowState.PlayRoute;
        onStateChange?.Invoke(currentState);
        
        cineCams[0].Priority = 0;
        cineCams[1].Priority = 1;
    }

    public void ArrangeRoute()
    {
        currentState = FlowState.ArrangeRoute;
        onStateChange?.Invoke(currentState);

        cineCams[0].Priority = 1;
        cineCams[1].Priority = 0;
    }
}
