using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : LivingEntity
{
    public readonly static int IdDie = Animator.StringToHash("Die");
    public AudioClip hitClip;
    public AudioClip deathClip;
    private AudioSource playerAudioSource;
    private Animator playerAnimator;
    private PlayerMovement playerMovement;
    private PlayerInput playerInput;
    public Image uiHealth;


    private void Awake()
    {
        playerAudioSource = GetComponent<AudioSource>();
        playerAnimator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        playerInput = GetComponent<PlayerInput>();

    }
    protected override void OnEnable()
    {
        base.OnEnable();
        
        playerMovement.enabled = true;
        playerInput.enabled = true;

        uiHealth.gameObject.SetActive(true);
        SetHealthUI();
    }
    

    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if(!IsDead) playerAudioSource.PlayOneShot(hitClip);
        base.OnDamage(damage, hitPoint, hitNormal);
        SetHealthUI();
        // Debug.Log(Health);
    }
    public override void Die()
    {
        base.Die();

        playerAnimator.SetTrigger(IdDie);
        playerAudioSource.PlayOneShot(deathClip);
        playerMovement.enabled = false;
        playerInput.enabled = false; 

        uiHealth.enabled = true;
        uiHealth.gameObject.SetActive(false);
    }
        
    public void SetHealthUI() => uiHealth.fillAmount = Health / startingHeath;

}
