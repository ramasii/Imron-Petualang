using UnityEngine;

public class Consumable : Item
{
    public bool dropOnUse = true;

    public override void Use()
    {
        base.Use();

        if (itemName == "daging")
        {
            Debug.Log("All enemies stopped chasing!");

            Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);

            foreach (Enemy enemy in enemies)
            {
                enemy.StopChase(6f);
            }
        }

        if (dropOnUse)
        {
            gameObject.SetActive(false);
        }
    }
}