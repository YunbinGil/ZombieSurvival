using UnityEngine;
using UnityEngine.Events;

public class LivingEntity : MonoBehaviour, IDamagable
{
    public float startingHeath =100f;
    public float Health{get; private set;}
    public bool IsDead{get; private set;}
    public UnityEvent OnDead; //죽었을때 호출할 함수 등록 가능.

    //재사용을 위한 초기화
    //virtual: 플레이어 / 좀비인지에 따라 달라질 수 있기 때문에
    protected virtual void OnEnable() 
    {
        Health = startingHeath;
        IsDead = false;
    }

    public virtual void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if(IsDead) return;
        Health -= damage;
        if (Health <= 0f)
        {
            Health = 0f;
            Die();
        }
    }


    //죽었을 때 호출
    public virtual void Die()
    {
        IsDead = true;
        OnDead.Invoke(); //등록된 함수 호출
    }
}
