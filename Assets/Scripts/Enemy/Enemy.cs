using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform[] points;
    public float speed = 3f;
    public float chaseSpeed = 5f;
    public float rotateSpeed = 5f;
    public float waitTime = 1f;
    public bool loop = true;

    private Vector3 spawnPos;

    private int currentPoint = 0;
    private bool isWaiting = false;
    private bool canMove = false;

    [HideInInspector]
    public bool isChasing = false;

    [HideInInspector]
    public Transform playerTarget;

    private void Start()
    {
        spawnPos = transform.localPosition;
    }

    void Update()
    {
        if (!canMove) return;

        // ======================
        // CHASE PLAYER
        // ======================
        if (isChasing && playerTarget != null)
        {
            RotateTo(playerTarget.position);

            transform.position = Vector3.MoveTowards(
                transform.position,
                playerTarget.position,
                chaseSpeed * Time.deltaTime
            );

            return;
        }

        // ======================
        // PATROL
        // ======================
        if (points.Length == 0 || isWaiting) return;

        Transform target = points[currentPoint];

        RotateTo(target.position);

        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            speed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, target.position) < 0.05f)
        {
            StartCoroutine(WaitAtPoint());
        }
    }

    void RotateTo(Vector3 targetPos)
    {
        Vector3 direction = targetPos - transform.position;
        direction.y = 0;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotateSpeed * Time.deltaTime
            );
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Serang");
        }
    }

    IEnumerator WaitAtPoint()
    {
        isWaiting = true;

        yield return new WaitForSeconds(waitTime);

        currentPoint++;

        if (currentPoint >= points.Length)
        {
            if (loop)
                currentPoint = 0;
            else
                enabled = false;
        }

        isWaiting = false;
    }

    IEnumerator MoveU()
    {
        Vector3 start = transform.localPosition;
        float t = 0;

        while (t < 1f)
        {
            t += Time.deltaTime * 2;

            Vector3 pos = Vector3.Lerp(start, spawnPos, t);

            pos.y += Mathf.Sin(t * Mathf.PI) * Mathf.Clamp(Vector3.Distance(start, spawnPos) * 0.5f, 0, 5f);

            transform.localPosition = pos;

            yield return null;
        }

        transform.localPosition = spawnPos;
    }

    public void PlayStop()
    {
        if (canMove)
        {
            canMove = false;
            StartCoroutine(MoveU());
        }
        else
        {
            canMove = true;
        }
    }
}