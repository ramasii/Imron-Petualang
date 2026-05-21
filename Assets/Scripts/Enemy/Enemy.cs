using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform[] points;
    public float speed = 3f;
    public float chaseSpeed = 5f;
    public float rotateSpeed = 5f;
    public float waitTime = 1f; //.
    public bool loop = true;
    [Header("Animator")]
    // public Animator animator;

    private Vector3 spawnPos;

    private int currentPoint = 0;
    private bool isWaiting = false;
    private bool canMove = false;
    private Vector3 moveDir;

    [HideInInspector]
    public bool isChasing = false;
    public bool canChase = true;

    [HideInInspector]
    public Transform playerTarget;

    Gameflow gameflow;

    private void Start()
    {
        spawnPos = transform.localPosition;
        gameflow = FindAnyObjectByType<Gameflow>();
        if (gameflow != null)
        {
            gameflow.onStateChange += OnGameflowStateChange;
        }
    }

    void Update()
    {
        if (!canMove && !canChase) return;

        // ======================
        // CHASE PLAYER
        // ======================
        if (canChase && isChasing && playerTarget != null)
        {
            RotateTo(playerTarget.position);
            moveDir = Vector3.MoveTowards(
                transform.position,
                playerTarget.position,
                chaseSpeed * Time.deltaTime
            );
            transform.position = moveDir;

            return;
        }

        // ======================
        // PATROL
        // ======================
        if (points.Length == 0 || isWaiting) return;

        Transform target = points[currentPoint];

        RotateTo(target.position);

        moveDir = Vector3.MoveTowards(
            transform.position,
            target.position,
            speed * Time.deltaTime
        );
        transform.position = moveDir;

        if (Vector3.Distance(transform.position, target.position) < 0.05f)
        {
            moveDir = Vector3.zero;
            StartCoroutine(WaitAtPoint());
        }
    }
    //     // =======================
    //     // ANIMATOR
    //     // =======================
    //     MoveAnim();
    // }

    // void MoveAnim()
    // {
    //     if (animator != null)
    //     {
    //         animator.SetFloat("Magnitude", moveDir.magnitude);
    //     }
    // }

    void RotateTo(Vector3 targetPos)
    {
        Vector3 direction = targetPos - transform.position;

        float angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;

        Vector3 rot = transform.eulerAngles;
        rot.y = -angle + 90f;

        transform.eulerAngles = rot;
    }

    void OnGameflowStateChange(FlowState newState)
    {
        PlayStop(newState);
    }

    private void OnTriggerEnter(Collider other)
    {
        // jika menyentuh player maka serang player (kembali ke arrange route)
        if (other.CompareTag("Player"))
        {
            // Debug.Log($"{gameObject.name} menyerang Player");

            // kembali ke arrange route setelah menyerang player
            Gameflow gameflow = FindAnyObjectByType<Gameflow>();
            if (gameflow != null) gameflow.ArrangeRoute();
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

    public void DeactivateEnemy()
    {
        gameObject.SetActive(false);
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

    void PlayStop(FlowState newState)
    {
        // kalo arrange route, musuh stop dan balik ke posisi spawn
        if (newState == FlowState.ArrangeRoute)
        {
            gameObject.SetActive(true); // pastikan musuh aktif saat arrange route

            canMove = false;
            StartCoroutine(MoveU());
            transform.position = spawnPos;
        }
        // kalo play route, musuh bisa jalan lagi
        else
        {
            canMove = true;
        }
    }

    public void StopChase(float duration)
    {
        StartCoroutine(StopChaseCoroutine(duration));
    }

    IEnumerator StopChaseCoroutine(float duration)
    {
        canChase = false;
        isChasing = false;
        playerTarget = null;

        yield return new WaitForSeconds(duration);

        canChase = true;
    }
}