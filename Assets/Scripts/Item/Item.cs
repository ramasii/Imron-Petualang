using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(BoxCollider))]
public class Item : MonoBehaviour
{
    public string itemName;
    public ItemPickupBtn pickupBtn;
    public Transform tileSource;
    private Rigidbody rb;
    private Vector3 initialPosition;
    private Vector3 initialRotation;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        // Subscribe to Gameflow state change event
        Gameflow gameflow = FindAnyObjectByType<Gameflow>();
        if (gameflow != null)
        {
            gameflow.onStateChange += OnGameflowStateChange;
        }
    }

    void Start()
    {
        if (pickupBtn != null)
        {
            pickupBtn.gameObject.SetActive(false); // Sembunyikan tombol saat item muncul
        }

        initialPosition = transform.localPosition;
        initialRotation = transform.localRotation.eulerAngles;
    }

    void OnGameflowStateChange(FlowState newState)
    {
        ResetItem(newState);
    }

    private void OnValidate()
    {
        if (!TryGetComponent<Rigidbody>(out rb))
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }

        rb.isKinematic = true; 
    }

    void ResetItem(FlowState newState)
    {
        if (newState == FlowState.ArrangeRoute)
        {
            Drop(true, false);
            gameObject.SetActive(true);
            transform.localPosition = initialPosition;
            transform.localRotation = Quaternion.Euler(initialRotation);
            rb.isKinematic = true;
        }
        else
        {
            rb.isKinematic = false; 
        }

        ShowPickupBtn(false); // Sembunyikan tombol saat direset
    }

    virtual public void Use()
    {
        Debug.Log("Using " + itemName);
    }

    virtual public void Drop(bool isKinematic = false, bool showBtn = true)
    {
        Debug.Log("Dropping " + itemName);
        transform.SetParent(tileSource, true); // Set parent ke tile source saat dijatuhkan

        if (rb != null)
        {
            rb.isKinematic = isKinematic; // Aktifkan fisika saat dijatuhkan
        }

        ShowPickupBtn(showBtn); // Tampilkan tombol saat dijatuhkan
    }

    virtual public void PickUp()
    {
        Debug.Log("Picking up " + itemName);

        if (rb != null)
        {
            rb.isKinematic = true; // Nonaktifkan fisika saat dipegang
        }
        if(itemName == "Dagger")
        {
            Player player = FindAnyObjectByType<Player>();
            player.equipWeapon = true;
        }

        ShowPickupBtn(false); // Sembunyikan tombol saat dipegang
    }

    public void ShowPickupBtn(bool b)
    {
        if (pickupBtn != null)
        {
            pickupBtn.gameObject.SetActive(b);

            Player player = FindAnyObjectByType<Player>();
            if(player != null && b == false)
            {
                player.CanUseItem(true); // Aktifkan penggunaan item saat tombol mati
            }
        }
    }
}
