using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public enum State
    {
        Ready,
        Empty,
        Reloading
    }

    public State CurrentState {get; private set;} //기본값은 Ready

    public Transform fireTransform;
    public ParticleSystem muzzleFlashEffect;
    public ParticleSystem shellEjectFlashEffect;
    private LineRenderer bulletLineRenderer;
    private AudioSource gunAudioPlayer;
    public AudioSource reloadPlayer;
    public GunData gunData;

    private float fireDistance = 50f;
    private int ammoRemain;
    private int magAmmo;
    private float lastFireTime;
    public UIManager uiManager;

    private void Awake()
    {
        bulletLineRenderer = GetComponent<LineRenderer>();
        gunAudioPlayer = GetComponent<AudioSource>();

        bulletLineRenderer.positionCount = 2;
        bulletLineRenderer.enabled = false;
    }

    private void OnEnable()
    {
        ammoRemain = gunData.startAmmoRemain;
        magAmmo = gunData.magCapcity;
        uiManager.SetAmmoText(magAmmo, ammoRemain);

        CurrentState = State.Ready;   
        lastFireTime = 0f;
    }

    public void Fire() //사용자입력-Fire1눌렸을때 호출될 함수
    {
        if(CurrentState == State.Ready && Time.time >= lastFireTime + gunData.timeBetFire)
        {
            lastFireTime = Time.time;
            Shoot();
        }        
    }
    public LayerMask targetLayer;
    private void Shoot() //실제로 총알 한 발 나갈때 호출
    {

        Vector3 hitPosition;
        //히트스캔방식 
        Ray ray = new Ray(fireTransform.position, fireTransform.forward);
        if(Physics.Raycast(ray, out RaycastHit hit, fireDistance, targetLayer))
        {
            hitPosition = hit.point;
            IDamagable target = hit.collider.GetComponent<IDamagable>();
            if (target != null)
            {
                target.OnDamage(gunData.damage, hit.point, hit.normal);
            }
        }
        else
        {
            hitPosition = fireTransform.position + fireTransform.forward * fireDistance;
        }
        StartCoroutine(ShotEffect(hitPosition));

        magAmmo--;
        uiManager.SetAmmoText(magAmmo, ammoRemain);

        if(magAmmo == 0)
        {
            CurrentState = State.Empty;
        }
    }

    private IEnumerator ShotEffect(Vector3 hitPosition)
    {
        muzzleFlashEffect.Play();
        shellEjectFlashEffect.Play();
        gunAudioPlayer.PlayOneShot(gunData.shotClip);

        bulletLineRenderer.SetPosition(0, fireTransform.position);
        bulletLineRenderer.SetPosition(1, hitPosition);
        bulletLineRenderer.enabled = true;
        yield return new WaitForSeconds(0.03f);
        bulletLineRenderer.enabled = false;

    }

    public bool Reload()
    {
        if(CurrentState == State.Reloading || ammoRemain==0 || magAmmo==gunData.magCapcity ) return false;
        // CurrentState = State.Reloading; 
        
        StartCoroutine(ReloadRoutine());

        return true;
    }
 
    private IEnumerator ReloadRoutine()
    {
        CurrentState = State.Reloading; 
        // Debug.Log("state: reloading");

        gunAudioPlayer.PlayOneShot(gunData.reloadClip);
        
        yield return new WaitForSeconds(gunData.reloadTime);
        
        int amount = Mathf.Min(ammoRemain, gunData.magCapcity-magAmmo);
        magAmmo += amount;
        ammoRemain -= amount;
        uiManager.SetAmmoText(magAmmo, ammoRemain);
        
        CurrentState = State.Ready;
        // Debug.Log("state: ready");
    }    

}
