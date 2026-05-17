using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSlider : MonoBehaviour
{
    public List<Tile> tiles = new List<Tile>();
    public float speed = 3f;
    public delegate void OnTimelineChanged(int newTimeline);
    public event OnTimelineChanged onTimelineChanged;

    [Header("Private")]
    [SerializeField] private int timeline = 0;

    private bool isSliding;

    void Start()
    {
        foreach (Tile tile in tiles)
        {
            tile.setStayPosition(tile.transform.position);
        }
    }

    public void SlideRight()
    {
        if (tiles.Count == 0 || isSliding) return;

        isSliding = true;

        List<Vector3> tempPositions = getTilePositions();
        Tile lastTile = tiles[tiles.Count - 1];

        tiles.RemoveAt(tiles.Count - 1);
        tiles.Insert(0, lastTile);

        for (int i = 0; i < tiles.Count; i++)
        {
            Vector3 target = tempPositions[i];

            tiles[i].setStayPosition(target);
        }

        StartCoroutine(UnlockAfterDelay());
    }

    public void SlideLeft()
    {
        if (tiles.Count == 0 || isSliding) return;

        isSliding = true;

        List<Vector3> tempPositions = getTilePositions();
        Tile firstTile = tiles[0];

        tiles.RemoveAt(0);
        tiles.Add(firstTile);

        for (int i = 0; i < tiles.Count; i++)
        {
            Vector3 target = tempPositions[i];

            tiles[i].setStayPosition(target);
        }
        StartCoroutine(UnlockAfterDelay());
    }

    public List<Vector3> getTilePositions()
    {
        List<Vector3> tilePositions = new List<Vector3>();

        foreach (Tile tile in tiles)
        {
            tilePositions.Add(tile.stayPosition);
        }

        return tilePositions;
    }

    IEnumerator UnlockAfterDelay()
    {
        yield return new WaitForSeconds(0.6f);
        isSliding = false;
    }

    public void SetTimeline(int t)
    {
        Debug.Log($"TileSlider {gameObject.name} timeline set to {t}");
        timeline = t;
        onTimelineChanged?.Invoke(timeline);
    }

    public int GetTimeline()
    {
        return timeline;
    }
}