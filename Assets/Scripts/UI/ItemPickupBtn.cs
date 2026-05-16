using UnityEngine;

public class ItemPickupBtn : MonoBehaviour
{
    public float heightFromItem = 1.5f;
    private Player player;
    private Item item;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = FindAnyObjectByType<Player>();
        item = GetComponentInParent<Item>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void LateUpdate()
    {
        if (item == null)
        {
            Destroy(gameObject);
            return;
        }

        // posisikan tombol di atas item
        Vector3 targetPos = item.transform.position + Vector3.up * heightFromItem;
        transform.position = targetPos;
    }

    public void OnClick()
    {
        player.PickUpItem(item);
    }
}
