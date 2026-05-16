using UnityEngine;

public class Weapon : Item
{
    public int damage;

    public override void Use()
    {
        base.Use();
        // Implementasi serangan atau efek senjata
        Debug.Log("Attacking with " + itemName + " for " + damage + " damage.");
    }
}
