using UnityEngine;
using System.Collections.Generic;

public class TimeSwapper : MonoBehaviour
{
    public List<TileSlider> tileSliders = new List<TileSlider>();
    
    public float swapAnimationSpeed = 5f;
    [SerializeField] List<Vector3> sliderPositions = new List<Vector3>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // set timeline awal
        for (int i = 0; i < tileSliders.Count; i++)
        {
            if(tileSliders[i])
            {
                tileSliders[i].SetTimeline(i);
                sliderPositions.Add(tileSliders[i].transform.position);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < tileSliders.Count; i++)
        {
            if (tileSliders[i])
            {
                if(tileSliders[i].transform.position != sliderPositions[i])
                {
                    AnimateSlide(tileSliders[i], sliderPositions[i]);
                }
            }
        }
    }

    public void SwapTime(int idxA, int idxB)
    {
        // Logika untuk swap time
        // tukar urutan tileSliders[idxA] dan tileSliders[idxB].
        Debug.Log($"tukar {idxA} >< {idxB}");

        // ambil posisi tile dari tileSliders[idxA] dan tileSliders[idxB]
        List<Vector3> tempTilePosA = tileSliders[idxA].getTilePositions();
        List<Vector3> tempTilePosB = tileSliders[idxB].getTilePositions();

        // (sliderPositions[idxA], sliderPositions[idxB]) = (sliderPositions[idxB], sliderPositions[idxA]);
        (tileSliders[idxB], tileSliders[idxA]) = (tileSliders[idxA], tileSliders[idxB]);

        // geser posisi tile sesuai urutan baru
        for (int i = 0; i < tileSliders[idxA].tiles.Count; i++)
        {
            tileSliders[idxA].tiles[i].setStayPosition(tempTilePosA[i]);
        }
        for (int i = 0; i < tileSliders[idxB].tiles.Count; i++)
        {
            tileSliders[idxB].tiles[i].setStayPosition(tempTilePosB[i]);
        }

        // tukar juga timeline nya
        int tempTimeline = tileSliders[idxA].GetTimeline();
        tileSliders[idxA].SetTimeline(tileSliders[idxB].GetTimeline());
        tileSliders[idxB].SetTimeline(tempTimeline);
    }

    void AnimateSlide(TileSlider t, Vector3 targetPosition)
    {
        t.transform.position = Vector3.MoveTowards(t.transform.position, targetPosition, Time.deltaTime * swapAnimationSpeed);
    }
}
