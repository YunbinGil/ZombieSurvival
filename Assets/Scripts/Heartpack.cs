using UnityEngine;

public class Heartpack : UsableItem
{
    public int healingAmount = 50;
    private PlayerHealth playerHealth;
    public override void Use(GameObject target)
    {
        playerHealth = target.GetComponent<PlayerHealth>();
        playerHealth.AddHealth(healingAmount);
        playerHealth.SetHealthUI();

    }

}
