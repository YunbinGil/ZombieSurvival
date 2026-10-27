using UnityEngine;

public class HealthPack : MonoBehaviour, IItem
{
    public int heal = 50;

    public void Use(GameObject target)
    {
        LivingEntity livingEntity = target.GetComponent<LivingEntity>();
        if (livingEntity != null)
        {
            livingEntity.AddHealth(heal);
        }
        Destroy(gameObject);
    }
}