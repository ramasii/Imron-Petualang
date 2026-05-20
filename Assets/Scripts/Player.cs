using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class Player : MonoBehaviour
{
    public bool canMove = false;
    public bool canUseitem = true;
    public bool equipWeapon, isAttacking;
    public float moveSpeed = 5f;
    public Item equippedItem;
    public Transform itemHoldPoint;
    public bool isGrounded = true;
    [Header("Animator")]
    public Animator animator;
    [Header("Private")]
    [SerializeField] private Vector3 spawnPoint;
    [SerializeField] private Vector3 spawnRotation;
    Camera mainCam;
    Vector2 moveInput;

    float footstepTimer;
    public float footstepDelay = 0.5f;

    Gameflow gameflow;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnPoint = transform.position;
        spawnRotation = transform.rotation.eulerAngles;
        mainCam = Camera.main;

        // Subscribe to Gameflow state change event
        gameflow = FindAnyObjectByType<Gameflow>();
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

        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            DropItem();
        }

        HandleFootstep();
}

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    void Move()
    {
        // mencegah pergerakan jika canMove false
        if (!canMove) return;

        MoveAnim();

        // mencegah pergerakan jika input terlalu kecil
        if (moveInput.sqrMagnitude < 0.01f) return;

        // ambil arah kamera
        Vector3 camForward = Vector3.ProjectOnPlane(mainCam.transform.forward, Vector3.up).normalized;
        Vector3 camRight = Vector3.ProjectOnPlane(mainCam.transform.right, Vector3.up).normalized;

        // gerak berdasarkan arah kamera
        Vector3 move = camForward * moveInput.y + camRight * moveInput.x;
        transform.position += moveSpeed * Time.deltaTime * move;
    }

    void MoveAnim()
    {
        // set animator parameter
        if (animator != null)
        {
            animator.SetFloat("Magnitude", moveInput.magnitude);
        }
    }

    public void Jump()
    {
        if (isGrounded)
        {
            // Implement jump logic here (e.g., add upward force)
            // Rigidbody rb = GetComponent<Rigidbody>();
            // if (rb != null)
            // {
            //     rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            // }
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            UseItem();
        }
    }

    // panggil method Use() dari item yang sedang dipegang
    void UseItem()
    {
        if (equippedItem != null && canUseitem)
        {
            equippedItem.Use();

            if (animator != null)
            {
                // jika item yang digunakan adalah consumable
                if (equippedItem is Consumable)
                {
                    animator.SetTrigger("Use Item");
                    DropItem();
                }
                // jika item yang digunakan adalah weapon
                else
                {
                    animator.SetTrigger("Attack");
                }
            }
        }
    }

    /// <summary>
    /// Method ini dipanggil dari Animation Event di animasi serangan. Pastikan nama method ini sesuai dengan yang ada di Animation Event.
    /// </summary>
    public void OnStartAttackAnim()
    {
        Debug.Log("Attack animation started");
        if (equippedItem is Weapon weapon)
        {
            weapon.EnableHitTrigger(true);
            isAttacking = true;
        }
    }

    /// <summary>
    /// Method ini dipanggil dari Animation Event di animasi serangan. Pastikan nama method ini sesuai dengan yang ada di Animation Event.
    /// </summary>
    public void OnEndAttackAnim()
    {
        if (equippedItem is Weapon weapon)
        {
            weapon.EnableHitTrigger(false);
            isAttacking = false;
        }
    }

    void DropItem(bool itemKinematic = false, bool showBtn = true)
    {
        if (equippedItem != null)
        {
            equippedItem.Drop(itemKinematic, showBtn);
            equippedItem = null;
        }
    }

    public void PickUpItem(Item item)
    {
        if (equippedItem != null)
        {
            DropItem();
        }

        item.PickUp();

        equippedItem = item;
        item.transform.SetParent(itemHoldPoint);
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;
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
            DropItem(true, false);
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
    }

    public void CanUseItem(bool canUse)
    {
        canUseitem = canUse;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            Debug.Log("Player entered item trigger");
            Item item = other.GetComponent<Item>();
            if (item != null && equippedItem != item)
            {
                item.ShowPickupBtn(true);
            }
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            Item item = other.GetComponent<Item>();
            if (item != null)
            {
                item.ShowPickupBtn(false);
            }

        }
    }

    void PlayFootstepSFX()
    {
        RaycastHit hit;

        if(Physics.Raycast(transform.position, Vector3.down, out hit, 5f))
        {
            Tile tile = hit.collider.GetComponent<Tile>();

            if (tile == null)
            {
                tile = hit.collider.GetComponentInParent<Tile>();
            }
            if (tile != null)
            {
                if(AudioManager.Instance != null)
                {
                    AudioManager.Instance.PlayFootstep(tile.tileType);
                }
                Debug.Log(hit.collider.name);
            }
        }
    }

    void HandleFootstep()
    {
        if (!canMove) return;

        bool isMoving = moveInput.sqrMagnitude > 0.01f;

        if (!isMoving) return;

        footstepTimer -= Time.deltaTime;

        if (footstepTimer <= 0f)
        {
            PlayFootstepSFX();
            footstepTimer = footstepDelay;
        }
    }
}
