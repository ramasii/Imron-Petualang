using UnityEngine;
using UnityEngine.UI;
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
    
    [Tooltip("Index [0] untuk ArrangeRoute, Index [1] untuk PlayRoute")]
    public List<Button> flowButtons;
    public delegate void OnStateChange(FlowState newState);
    public event OnStateChange onStateChange;

    public void PlayRoute()
    {
        currentState = FlowState.PlayRoute;
        onStateChange?.Invoke(currentState);

        // aktifin button play route, matiin button arrange route
        flowButtons[0].gameObject.SetActive(true);
        flowButtons[1].gameObject.SetActive(false);

        cineCams[0].Priority = 0;
        cineCams[1].Priority = 1;
    }

    public void ArrangeRoute()
    {
        currentState = FlowState.ArrangeRoute;
        onStateChange?.Invoke(currentState);

        // aktifin button arrange route, matiin button play route
        flowButtons[0].gameObject.SetActive(false);
        flowButtons[1].gameObject.SetActive(true);

        cineCams[0].Priority = 1;
        cineCams[1].Priority = 0;
    }
}
