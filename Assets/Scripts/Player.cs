using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class Player : MonoBehaviour
{
    public bool canMove = false;
    public float moveSpeed = 5f;
    [SerializeField] private Vector3 spawnPoint;
    [SerializeField] private Vector3 spawnRotation;
    Camera mainCam;
    Vector2 moveInput;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnPoint = transform.position;
        spawnRotation = transform.rotation.eulerAngles;
        mainCam = Camera.main;

        // Subscribe to Gameflow state change event
        Gameflow gameflow = FindAnyObjectByType<Gameflow>();
        if (gameflow != null)
        {
            gameflow.onStateChange += OnGameflowStateChange;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        LookAtCursor();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    void Move()
    {
        // mencegah pergerakan jika canMove false
        if (!canMove) return;

        // mencegah pergerakan jika input terlalu kecil
        if (moveInput.sqrMagnitude < 0.01f) return;

        // ambil arah kamera
        Vector3 camForward = Vector3.ProjectOnPlane(mainCam.transform.forward, Vector3.up).normalized;
        Vector3 camRight = Vector3.ProjectOnPlane(mainCam.transform.right, Vector3.up).normalized;

        // gerak berdasarkan arah kamera
        Vector3 move = camForward * moveInput.y + camRight * moveInput.x;
        transform.position += moveSpeed * Time.deltaTime * move;
    }

    void LookAtCursor()
    {
        // mencegah pergerakan jika canMove false
        if (!canMove) return;

        // Implementation for looking at cursor
        Ray ray = mainCam.ScreenPointToRay(Mouse.current.position.ReadValue());
        Plane groundPlane = new Plane(Vector3.up, transform.position);
        if (groundPlane.Raycast(ray, out float hitDistance))
        {
            Vector3 hitPoint = ray.GetPoint(hitDistance);
            Vector3 lookDirection = (hitPoint - transform.position).normalized;
            lookDirection.y = 0; // Keep the player upright
            if (lookDirection.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
            }
        }
    }


    void OnGameflowStateChange(FlowState newState)
    {
        if (newState == FlowState.ArrangeRoute)
        {
            ResetPlayer();
            canMove = false;
        }
        else if (newState == FlowState.PlayRoute)
        {
            // Additional logic for PlayRoute state can be added here if needed
            canMove = true;
        }
    }

    void ResetPlayer()
    {
        ResetPosition();
        transform.rotation = Quaternion.Euler(spawnRotation);
    }

    public void ResetPosition()
    {
        StartCoroutine(MoveU());
    }
    
    IEnumerator MoveU()
    {
        // isAnimating = true;

        Vector3 start = transform.position;
        float t = 0;

        while (t < 1f)
        {
            t += Time.deltaTime * 2;
            Vector3 pos = Vector3.Lerp(start, spawnPoint, t);
            pos.y += Mathf.Sin(t * Mathf.PI) * Mathf.Clamp(Vector3.Distance(start, spawnPoint) * 0.5f, 0, 5f); // Adjust the height of the arc based on distance
            transform.position = pos;

            yield return null;
        }

        transform.position = spawnPoint;

        //  penting: sync posisi + stop blocking
        //stayPosition = stayPosition;
        // isAnimating = false;
    }
}
