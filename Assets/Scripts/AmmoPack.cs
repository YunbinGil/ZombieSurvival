using UnityEngine;

public class AmmoPack : UsableItem
{
    private Gun gun;
    public int ammoAmount = 50;
    public override void Use(GameObject target)
    {
        gun = target.GetComponent<PlayerShooter>().gun;
        gun.AddAmmo(ammoAmount);
    }
}
