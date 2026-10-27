using UnityEngine;

public class Heartpack : UsableItem
{
    public int healingAmount = 50;
    private LivingEntity livingEntity;
    public override void Use(GameObject target)
    {
        livingEntity = target.GetComponent<LivingEntity>();
        if (livingEntity != null)
        {
            livingEntity.AddHealth(healingAmount);
        }
    }
}
