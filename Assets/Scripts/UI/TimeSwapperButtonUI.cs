using UnityEngine;
using System.Collections.Generic;

public class TimeSwapperButtonUI : MonoBehaviour
{
    public TimeSwapper timeSwapper;
    public Vector2Int swapIdx; // (idxA, idxB)
    public List<TileSliderUI> tileSliderBtns = new List<TileSliderUI>();

    public void SwapTime()
    {
        timeSwapper.SwapTime(swapIdx.x, swapIdx.y);
        (tileSliderBtns[swapIdx.x].tileSlider, tileSliderBtns[swapIdx.y].tileSlider) =
            (tileSliderBtns[swapIdx.y].tileSlider, tileSliderBtns[swapIdx.x].tileSlider);
    }
}
