using UnityEngine;

public class Weapon : Item
{
    public int damage;
    public Collider weaponHitTrigger;

    public override void Use()
    {
        base.Use();
        // Implementasi serangan atau efek senjata
        // Debug.Log("Attacking with " + itemName + " for " + damage + " damage.");
    }

    public void EnableHitTrigger(bool enabled)
    {
        // Debug.Log("Weapon hit trigger " + (enabled ? "enabled" : "disabled"));
        weaponHitTrigger.enabled = enabled;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Debug.Log("Hit enemy with " + itemName);
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.DeactivateEnemy();
            }
        }
    }
}
