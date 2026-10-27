using NUnit.Framework;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : LivingEntity
{
    public enum Status
    {
        Idle,
        Trace,
        Attack,
        Die
    }
    public Transform target; //일정 거리안에없으면 null, 있으면 가장 가까운 go를 
    public LayerMask targetLayer;
    public float traceDistance = 10f; //유니티 단위 
    public float attackDistance = 1f;

    public ParticleSystem bloodEffect;
    public Collider zombieCollider;
    public Renderer zombieRenderer;
    public AudioClip hitClip;
    public AudioClip deathClip;

    private NavMeshAgent agent;
    private Animator zombieAnimator;
    private AudioSource zombieAudioSource;
    public float damage = 10;
    private float lastAttackTime;
    public float attackInterval = 3f;
    private Status currentStatus;
    public static readonly int IdHasTarget = Animator.StringToHash("HasTarget");
    public static readonly int IdDie = Animator.StringToHash("Die");

    public Status CurrentStatus
    {
        get => currentStatus;
        set
        {
            currentStatus = value;

            Debug.Log(currentStatus);
            switch (currentStatus)
            {
                case Status.Idle:
                    agent.isStopped = true; //agent그대로 정지 
                    //isStopped는 한번 수동할당하면 모든케이스 수동할당해야됨. 자동값안들어감.
                    zombieAnimator.SetBool(IdHasTarget, false);
                    break;
                case Status.Trace:
                    agent.isStopped = false; //agent 움직일 수 있도록
                    zombieAnimator.SetBool(IdHasTarget, true);
                    break;
                case Status.Attack:
                    zombieAnimator.SetBool(IdHasTarget, false);
                    agent.isStopped = true; 
                    break;
                case Status.Die:
                    agent.isStopped = true; 
                    zombieAnimator.SetTrigger(IdDie);
                    zombieCollider.enabled = false;
                    zombieAudioSource.PlayOneShot(deathClip);
                    break;
            }
        }
    }

    public void Setup(ZombieData data)
    {
        startingHealth = data.maxHp;
        damage = data.damage;
        agent.speed = data.speed; //유니티 단위 (초속)
        zombieRenderer.material.color = data.skinColor;
    }
    
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        zombieAnimator = GetComponent<Animator>();
        zombieAudioSource = GetComponent<AudioSource>();
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        agent.enabled = true;
        

        //초기위치 (Nav Mesh 위에 있는 점이어야됨) 
        // NavMesh.SamplePosition(): 현재 씬의 NavMesh 위 점을 하나 리턴하게됨
        //첫번째 인자에서 가장 가까운 점을 return
        if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 10f, NavMesh.AllAreas))
        {
            agent.Warp(hit.position); //Warp: 순간이동 
        }//실패 처리 생략. (예외가 발생할 일이 없으셔)

        zombieCollider.enabled = true;

        CurrentStatus = Status.Idle; //프로퍼티로 할당해야 setter 코드가 실행됨

        lastAttackTime = 0f;
    }

    private void OnDisable()
    {
        if (agent.isOnNavMesh)
        {
            agent.ResetPath(); //이전 경로가 남아았으면 안됨.
        }
    }

    private void Update()
    {
        switch (CurrentStatus)
            {
                case Status.Idle:
                    UpdateIdle();
                    break;
                case Status.Trace:
                    UpdateTrace();
                    break;
                case Status.Attack:
                    UpdateAttack();
                    break;
                case Status.Die:
                    UpdateDie();
                    break;
            }
    }

    private void UpdateIdle() 
    {
        if(target != null && Vector3.Distance(transform.position, target.position) < traceDistance)
        {
            CurrentStatus = Status.Trace;
            return;
        }

        target = FindTarget(traceDistance); //매프레임할 필요는 ㄴㄴ
    }

    private void UpdateTrace()
    {

        if(target !=null && Vector3.Distance(transform.position, target.position) < attackDistance)
        {
            CurrentStatus = Status.Attack;
            return;
        }
        else if(target==null || Vector3.Distance(transform.position, target.position) > traceDistance)
        {
            target = null;
            CurrentStatus = Status.Idle;
            return;
        }

        agent.SetDestination(target.position); //매프레임할 필요는 ㄴㄴ
    }

    private void UpdateAttack()
    {
        if(target!=null && Vector3.Distance(transform.position, target.position) > attackDistance)
        {
            CurrentStatus = Status.Trace;
            return;
        }
        
        if(Time.time >= lastAttackTime + attackInterval)
        {
            // Debug.Log($"{lastAttackTime} / interval {attackInterval} / now: {Time.time}");
            IDamagable damagableTarget = target.GetComponent<IDamagable>();
            if (damagableTarget != null)
            {
                damagableTarget.OnDamage(damage, target.position, (transform.position- target.position).normalized);
                lastAttackTime = Time.time;
            }
        }       

    }
    private void UpdateDie()
    {
        
    }

    private Transform FindTarget(float radius)
    {
        //3D공간상의 가상의 구(중점, 반경)에 충돌되는 collider들을 배열로 만들어서 리턴해줌
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, targetLayer);

        Transform closet = null;
        float closetDistance =  float.MaxValue; //colliders.length가 0 일수도있어서 colliders[0]으로 할당하면 안됨.

        for(int i=0; i<colliders.Length; i++)
        {
            float distance = Vector3.Distance(transform.position, colliders[i].transform.position);
            if (distance < closetDistance)
            {
                closetDistance = distance;
                closet = colliders[i].transform;
            }
        }

        return closet;
    }
    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if(IsDead)
        {
            return;
        }

        base.OnDamage(damage, hitPoint, hitNormal);
        zombieAudioSource.PlayOneShot(hitClip);
        bloodEffect.transform.position = hitPoint;
        bloodEffect.transform.forward = hitNormal;
        bloodEffect.Play();
    }
    public override void Die()
    {
        base.Die();
        CurrentStatus = Status.Die;
    }
}
