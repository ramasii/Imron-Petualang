using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour
{
    public Vector3 stayPosition;
    public float slideSpeed = 50f;

    private bool isAnimating = false;

    void Update()
    {
        if (!isAnimating && transform.position != stayPosition)
        {
            AnimateSlide(stayPosition);
        }
    }

    public void setStayPosition(Vector3 newPos)
    {
        stayPosition = newPos;
    }

    void AnimateSlide(Vector3 targetPosition)
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            Time.deltaTime * slideSpeed
        );
    }

    // =========================
    //  U TRANSITION FIXED
    // =========================
    public void MoveU(Vector3 target, float curveHeight, float speed)
    {
        StartCoroutine(MoveUCorr(target, curveHeight, speed));
    }

    IEnumerator MoveUCorr(Vector3 target, float curveHeight, float speed)
    {
        isAnimating = true;

        Vector3 start = transform.position;
        float t = 0;

        while (t < 1f)
        {
            t += Time.deltaTime * speed;
            Vector3 pos = Vector3.Lerp(start, target, t);
            pos.y += -Mathf.Sin(t * Mathf.PI) * curveHeight;
            transform.position = pos;

            yield return null;
        }

        transform.position = target;

        //  penting: sync posisi + stop blocking
        stayPosition = target;
        isAnimating = false;
    }
}