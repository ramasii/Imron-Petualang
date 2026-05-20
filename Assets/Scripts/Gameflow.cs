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
    public delegate void OnLevelFinish();
    public event OnLevelFinish onLevelFinish;
    GameObject canvasGroup;

    void Start()
    {
        EndTransition();
        canvasGroup = GameObject.Find("TileUIManager");
    }

    void EndTransition()
    {
        Transition transition = FindAnyObjectByType<Transition>();
        if (transition)        {
            transition.EndTransition();
        }
    }

    public void PlayRoute()
    {
        currentState = FlowState.PlayRoute;
        onStateChange?.Invoke(currentState);

        // aktifin button play route, matiin button arrange route
        flowButtons[0].gameObject.SetActive(true);
        flowButtons[1].gameObject.SetActive(false);

        // matikan tile UI
        TileUIShow(false);

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

        // nyalakan tile UI
        TileUIShow(true);

        cineCams[0].Priority = 1;
        cineCams[1].Priority = 0;
    }

    void TileUIShow(bool show)
    {
        // nyalakan UI tile
        if (canvasGroup)
        {
            CanvasGroup cg = canvasGroup.GetComponent<CanvasGroup>();
            if (cg)
            {
                cg.alpha = show ? 1 : 0;
                cg.interactable = show;
                cg.blocksRaycasts = show;
            }
        }
        else
        {
            Debug.LogWarning("CanvasGroup not found in TileUIManager");
        }
    }
    
    public void FinishLevel()
    {
        onLevelFinish?.Invoke();
    }
}
