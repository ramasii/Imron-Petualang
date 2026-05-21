using UnityEngine;

public class KeyLike : Consumable
{
    public int numKey = 0;
    public JembatanGear targetObject;

    public override void Use()
    {
        base.Use();
        if (targetObject != null)
        {
            targetObject.AddKey();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<JembatanGear>(out JembatanGear gear))
        {
            if(gear.numKey == numKey)
            {
                dropOnUse = true;
                targetObject = gear;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.TryGetComponent<JembatanGear>(out JembatanGear gear))
        {
            if(gear.numKey == numKey)
            {
                dropOnUse = false;
                targetObject = null;
            }
        }
    }
}
