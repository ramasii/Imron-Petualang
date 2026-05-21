using UnityEngine;

public class Consumable : Item
{
    public bool dropOnUse = true;
    public override void Use()
    {
        base.Use();

        // implemen efek
        if(dropOnUse)
        {
            gameObject.SetActive(false);
        }
    }
}
