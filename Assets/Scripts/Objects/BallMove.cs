using UnityEngine;

public class BallMove : MonoBehaviour
{
    public float speed = 5f;
    private Gameflow gameflow;
    private bool canMove = false;
    private Vector3 initialPosition;

    void Start()
    {
        // Store the initial position

        gameflow = FindAnyObjectByType<Gameflow>();
        if (gameflow != null)
        {
            gameflow.onStateChange += OnStateChanged;
            canMove = (gameflow.currentState == FlowState.PlayRoute);
        }
        else
        {
            Debug.LogWarning("Gameflow not found for BallMove");
        }
    }

    void OnDestroy()
    {
        if (gameflow != null)
        {
            gameflow.onStateChange -= OnStateChanged;
        }
    }

    void OnStateChanged(FlowState state)
    {
        if (state == FlowState.PlayRoute)
        {
            initialPosition = transform.position;
            canMove = true;
        }
        else
        {
            canMove = false;
            ResetPosition();
        }
    }

    void Update()
    {
        if (!canMove) return;

        transform.position += Vector3.right * speed * Time.deltaTime;

        // Reset ball if it goes too far to the right
        if (transform.position.x > 7f)
        {
            ResetPosition();
        }
    }

    void ResetPosition()
    {
        transform.position = initialPosition;
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
}
