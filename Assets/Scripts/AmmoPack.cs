using UnityEngine;

public class AmmoPack : MonoBehaviour, IItem
{
    public int ammo = 50;

    public void Use(GameObject target)
    {
        PlayerShooter shooter = target.GetComponent<PlayerShooter>();
        if (shooter == null || shooter.gun == null)
        {
            return;
        }
        shooter.gun.AddAmmo(ammo);
        gameObject.SetActive(false);
    }
}