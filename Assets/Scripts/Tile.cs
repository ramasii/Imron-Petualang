using UnityEngine;

public class Tile : MonoBehaviour
{
    public Vector3 stayPosition;
    public float slideSpeed = 50f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position != stayPosition)
        {
            AnimateSlide(stayPosition);
        }
    }

    public void setStayPosition(Vector3 newPos)
    {
        // Debug.Log($"set stay pos {gameObject.name} {stayPosition} to {newPos}");
        stayPosition = newPos;
        // Debug.Log($"stay pos {gameObject.name} is now {stayPosition}");
    }
    
    void AnimateSlide(Vector3 targetPosition)
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * slideSpeed);
    }
}
