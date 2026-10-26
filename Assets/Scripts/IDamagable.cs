using UnityEngine;

public interface IDamagable
{
    //hitPoint 피격이펙트 위치, hitNormal 피격이펙트 방향
    void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal);

}
