using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tile : MonoBehaviour
{
    public Vector3 stayPosition;
    public float slideSpeed = 50f;
    public float uSpeed = 50f;
    public float scalSize = 3f;
    public float curveHeight = 2f;
    [Header("Private")]

    private bool isAnimating = false;
    private Vector3 lastStayPos;
    [SerializeField]private TileSlider parentSlider;

    public TileType tileType;

    public enum TileType
    {
        Grass,
        Stone
    }

    void Start()
    {
        // TileSlider parentSlider = GetComponentInParent<TileSlider>();
        if (parentSlider)
        {
            parentSlider.onTimelineChanged += OnTimelineChanged;
        }
    }

    void Update()
    {
        if (!isAnimating && transform.position != stayPosition)
        {
            // dicek kalo posisi X nya > skala
            if (Mathf.Abs(lastStayPos.x - stayPosition.x) > scalSize)
            {
                MoveU();
            }
            // kalo po posisi X nya ga jauh pindahnya
            else
            {
                // dicek lagi apakah posisi Z berubah
                // kalo iy berarti lagi swap time
                if (lastStayPos.z > stayPosition.z)
                {
                    MoveU();
                }
                // ini kalo slide tile tapi ga jauh banget
                else
                {
                    AnimateSlide();
                }
            }
        }
    }

    public void setStayPosition(Vector3 newPos)
    {
        lastStayPos = stayPosition; // disimpen posisi terakhir
        stayPosition = newPos;
    }

    void AnimateSlide()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            stayPosition,
            Time.deltaTime * slideSpeed
        );
    }

    // =========================
    //  U TRANSITION
    // =========================
    public void MoveU()
    {
        StartCoroutine(MoveUCorr());
    }

    IEnumerator MoveUCorr()
    {
        isAnimating = true;

        Vector3 start = transform.position;
        float t = 0;

        while (t < 1f)
        {
            t += Time.deltaTime * uSpeed;
            Vector3 pos = Vector3.Lerp(start, stayPosition, t);
            pos.y += -Mathf.Sin(t * Mathf.PI) * curveHeight;
            transform.position = pos;

            yield return null;
        }

        transform.position = stayPosition;

        //  penting: sync posisi + stop blocking
        //stayPosition = stayPosition;
        isAnimating = false;
    }

    public void OnTimelineChanged(int newTimeline)
    {
        Debug.Log($"Tile {gameObject.name} received timeline change to {newTimeline}");
        List<TimableObject> timableObjects = new List<TimableObject>(GetComponentsInChildren<TimableObject>());
        foreach (TimableObject obj in timableObjects)
        {
            Debug.Log($"Tile {gameObject.name} set phase {newTimeline} for {obj.gameObject.name}");
            obj.SetPhase(newTimeline);
        }
    }

    public int GetTimeline()
    {
        TileSlider parentSlider = GetComponentInParent<TileSlider>();
        if (parentSlider)
        {
            return parentSlider.GetTimeline();
        }

        return 0;
    }
}