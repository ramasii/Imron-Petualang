using UnityEngine;

public class Consumable : Item
{
    public override void Use()
    {
        base.Use();

        // implemen efek
        gameObject.SetActive(false);
    }
}
