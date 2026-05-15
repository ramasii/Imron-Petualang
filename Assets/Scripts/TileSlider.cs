using UnityEngine;
using System.Collections.Generic;

public class TileSlider : MonoBehaviour
{
    public List<Tile> tiles = new List<Tile>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Inisialisasi posisi tile
        foreach (Tile tile in tiles)
        {
            tile.setStayPosition(tile.transform.position);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SlideRight()
    {
        if (tiles.Count == 0) return;

        // ambil position sebelum digeser
        List<Vector3> tempPositions = getTilePositions();

        // geser urutan tile
        Tile lastTile = tiles[tiles.Count - 1];
        tiles.RemoveAt(tiles.Count - 1);
        tiles.Insert(0, lastTile);

        // geser posisi tile sesuai urutan baru
        for (int i = 0; i < tiles.Count; i++)
        {
            tiles[i].setStayPosition(tempPositions[i]);
        }
    }

    public void SlideLeft()
    {
        if (tiles.Count == 0) return;

        // ambil position sebelum digeser
        List<Vector3> tempPositions = getTilePositions();

        // geser urutan tile
        Tile firstTile = tiles[0];
        tiles.RemoveAt(0);
        tiles.Add(firstTile);

        // geser posisi tile sesuai urutan baruu
        for (int i = 0; i < tiles.Count; i++)
        {
            tiles[i].setStayPosition(tempPositions[i]);
        }
    }

    public void setTilePositions(List<Vector3> newPositions)
    {
        for (int i = 0; i < tiles.Count; i++)
        {
            tiles[i].setStayPosition(newPositions[i]);
        }
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
}
