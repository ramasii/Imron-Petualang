using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    public float ballSpeed = 5f;
    private BallMove ballMove;

    void Start()
    {
        // Find the BallMove component on this or child objects
        ballMove = GetComponent<BallMove>();
        if (ballMove == null)
        {
            ballMove = GetComponentInChildren<BallMove>();
        }

        if (ballMove != null)
        {
            ballMove.SetSpeed(ballSpeed);
        }
        else
        {
            Debug.LogWarning("BallMove component not found on BallSpawner or its children");
        }
    }
}

